using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using VirtualMachine.Assembly;
using VirtualMachine.Enumerations;
using VirtualMachine.Interfaces;
using VirtualMachine.Threading;
using VirtualMachine.Threading.States;
using VirtualMachine.VMTools;
using static VirtualMachine.VMTools.VM_Extensions;

namespace VirtualMachine
{
    public class VirtualMachine
    {
        private long ticks = 0;

        public Logger Logger;

        public Assembler Assembler;

        public ThreadQueue ThreadQueue;

        public Dictionary<string, int> SymbolTable = new Dictionary<string, int>(); // <label, address offset>

        //   public Dictionary<REGISTER, Int32> Registers = Enum.GetValues(typeof(REGISTER)).Cast<REGISTER>().ToList().Zip(Enumerable.Repeat(new Int32(), Enum.GetValues(typeof(REGISTER)).Length), (r, v) => new { r, v }).ToDictionary(i => i.r, i => i.v);

        public Dictionary<REGISTER, Int32> Registers = ZipEntries<REGISTER, Int32>();

        public Int32 PC { get => Registers[REGISTER.PC]; set => Registers[REGISTER.PC] = value; }
        public Int32 SL { get => Registers[REGISTER.SL]; set => Registers[REGISTER.SL] = value; }
        public Int32 SB { get => Registers[REGISTER.SB]; set => Registers[REGISTER.SB] = value; }
        public Int32 SP { get => Registers[REGISTER.SP]; set => Registers[REGISTER.SP] = value; }
        public Int32 FP { get => Registers[REGISTER.FP]; set => Registers[REGISTER.FP] = value; }
        public Thread MainThread { get => ThreadQueue[THREAD.Main]; set => ThreadQueue[THREAD.Main] = value; }
        public THREAD ActiveThread { get; set; }
        public int RegistersBlock { get => 4 * Registers.Count; }

        public int CodeSegmentSize { get; internal set; }
        public int DataSegmentSize { get; internal set; }
        public int HeapSegmentSize { get; internal set; }

        public static int MAX_THREAD { get; internal set; }

        public static int MEMORY_SIZE { get; } = 500000;

        public static int STACK_SIZE { get; } = 12000;


        public static int CHUNK { get; } = 4;


        public readonly TOKEN[] FLAGS = new TOKEN[MEMORY_SIZE];

        public readonly byte[] MEMORY = new byte[MEMORY_SIZE];

        public VirtualMachine(params string[] args)
        {
            Logger = new Logger();
            Assembler = new Assembler(this);
            Assembler.Run(args);
            ThreadQueue = new ThreadQueue(ThreadCount());
        //    Run();
        }

        private int Compare(int destination, int source) => destination > source ? int.MaxValue : destination < source ? int.MinValue : 0;
        private int Fetch(int offset = 0, bool increment = true) => BitConverter.ToInt32(MEMORY.Skip(offset + (increment ? (PC += 4) - 4 : PC)).Take(CHUNK).ToArray(), 0);
        private int FetchAddress(int location) => BitConverter.ToInt32(MEMORY.Skip(location - 3).Take(CHUNK).Reverse().ToArray(), 0);
        private int FetchByteAddress(int location) => MEMORY[location];
        private TOKEN FetchFlag(int offset = 0) => FLAGS[PC + offset];
        private byte[] StoreInt(int value) => BitConverter.GetBytes(value);
        private void StoreBytes(int registerLocation, int memoryLocation)
        {
            foreach (byte bite in StoreInt(Registers[(REGISTER)registerLocation]))
            {
                switch (FetchFlag(-4))
                {
                    case TOKEN.Register: // memory location indicated by the right-hand register value
                        FLAGS[Registers[(REGISTER)memoryLocation]] = TOKEN.DirInt;
                        MEMORY[Registers[(REGISTER)memoryLocation]--] = bite;
                        break;

                    case TOKEN.Label:
                        FLAGS[memoryLocation] = TOKEN.DirInt;
                        MEMORY[memoryLocation--] = bite;
                        break;
                }
            }
        }
        private void StoreByte(int destination, int source)
        {
            switch (FetchFlag(-4))
            {
                case TOKEN.Register:
                    FLAGS[Registers[(REGISTER)destination]] = TOKEN.DirByte;
                    MEMORY[Registers[(REGISTER)destination]] = BitConverter.GetBytes(Registers[(REGISTER)source]).First();
                    break;

                case TOKEN.Label:
                    FLAGS[source] = TOKEN.DirByte;
                    MEMORY[source] = MEMORY[Registers[(REGISTER)destination]];
                    break;
            }
        }
        private THREAD NextAvailableThread() => ThreadQueue.FirstOrDefault(t => !t.IsActive()).ID;

        private int ThreadCount() => (MEMORY_SIZE - CodeSegmentSize - DataSegmentSize - HeapSegmentSize) / STACK_SIZE;
    //    private ThreadQueue AllocateThreadStacks() => (ThreadQueue)ZipEnums<THREAD, Inactive, Thread>(CalculateThreadCount());
   //     private ThreadQueue AllocateThreadStacks() => (ThreadQueue)Enum.GetValues(typeof(THREAD)).Cast<THREAD>().ToList().Zip(Enumerable.Repeat(new Inactive(), CalculateThreadCount()), (id, thread) => new Thread(id)).ToList();
        private int StackOffset(THREAD thread) => MEMORY_SIZE - (int)thread * STACK_SIZE;
        private void StoreRegister(REGISTER register, int location) => StoreInt(Registers[register]).ToList().ForEach(bite => MEMORY[location--] = bite);
        private void StoreRegister(Int32 register, int location) => StoreInt(register).ToList().ForEach(bite => MEMORY[location--] = bite);
        private void LoadRegister(Int32 register, int location) => register = FetchAddress(location);
        private void LoadRegister(REGISTER register, int location) => Registers[register] = FetchAddress(location);
        private void TerminateThread(THREAD thread) => ThreadQueue[thread != THREAD.Main ? thread : throw new Exception("Cannot invoke END instruction on Main Thread!")].Deactivate();
        private THREAD NextActiveThread(THREAD thread) => NextActiveThread((int)thread);
        private THREAD NextActiveThread(int thread) => ThreadQueue[thread].IsActive() ? (THREAD)thread : NextActiveThread(thread + 1);

        private List<int> GetThings(int thread)
        {
            int offset = thread * STACK_SIZE;
            List<int> ret = new List<int>();
            for (int i = 0; i < STACK_SIZE/4; i++)
            {
                ret.Add(FetchAddress(MEMORY_SIZE - 4 - offset - (i * 4)));
            }
            return ret;
        }


        private void InitializeThread()
        {
            try
            {
                StoreThreadState(ActiveThread);
                ThreadQueue[ActiveThread = NextAvailableThread()].Activate();
            //    InitializeThreadRegisters(ActiveThread);

            }
            catch (Exception) { throw new StackOverflowException("No available threads!"); }

        }

        private void InitializeThreadRegisters(THREAD thread)
        {
            SB = MEMORY_SIZE - 4 - RegistersBlock - ((int)thread * STACK_SIZE);
            FP = SP = SB - 4;
            SL = MEMORY_SIZE - ((int)thread * STACK_SIZE);
        }

        private void StoreThreadState(THREAD thread)
        {
            int memloc = StackOffset(thread) - 4;
            foreach (KeyValuePair<REGISTER, Int32> register in Registers) { StoreRegister(register.Value, memloc); memloc -= 4; }
        }

        private void LoadThreadState(THREAD thread)
        {
            int memloc = StackOffset(thread) - 4;
            foreach (KeyValuePair<REGISTER, Int32> register in Registers) { LoadRegister(register.Key, memloc); memloc -= 4; }
        }

        private void ContextSwitch()
        {
            THREAD next = NextActiveThread(ActiveThread);
            if (ActiveThread == next) return;
            if (ThreadQueue[ActiveThread].Locked) return;
         //  if (next is THREAD.Main && ThreadQueue[next].Blocked) next = NextActiveThread(next);
            StoreThreadState(ActiveThread);
            ActiveThread = next;
            LoadThreadState(ActiveThread);
        }

        public void Run()
        {
            PC = 0;

            //  SL = MEMORY_SIZE - STACK_SIZE - 4;
            SB = MEMORY_SIZE - 4 - RegistersBlock;
            FP = SP = SB - 4;
            SL = MEMORY_SIZE - STACK_SIZE;
            //     SB = MEMORY_SIZE;
            //      FP = SP = SB - 4;
         //   InitializeThreadRegisters(ActiveThread);
            InitializeThread();
           // StoreThreadState(ActiveThread);

            bool done = false;
            while (!done)
            {
                ticks++;

                try
                {
                    switch (FetchFlag())
                    {
                        case TOKEN.Opcode:
                            switch ((OPCODE)Fetch())
                            {
                                #region No Commands
                                case OPCODE.NONE:
                                    break;
                                #endregion

                                #region Jump Commands
                                case OPCODE.JMP: // Branch to Label
                                    PC = Fetch();
                                    break;

                                case OPCODE.JMR: // Branch to address in source register
                                    PC = Registers[(REGISTER)Fetch()];
                                    //PC = FetchAddress(Registers[(REGISTER)Fetch()]);
                                    break;
                                #endregion

                                #region Branch Commands
                                case OPCODE.BNZ:
                                    // Branch to Label if source register is not zero
                                    // Source Register | Label Address
                                    if (Registers[(REGISTER)Fetch()] != 0) { PC = Fetch(); }
                                    else { Fetch(); }
                                    break;

                                case OPCODE.BGT:
                                    // Branch to Label if source register is greater than zero
                                    // Source Register | Label Address
                                    if (Registers[(REGISTER)Fetch()] > 0) { PC = Fetch(); }
                                    else { Fetch(); }
                                    break;

                                case OPCODE.BLT:
                                    // Branch to Label if source register is lower than zero
                                    // Source Register | Label Address
                                    if (Registers[(REGISTER)Fetch()] < 0) { PC = Fetch(); }
                                    else { Fetch(); }
                                    break;

                                case OPCODE.BRZ:
                                    // Branch to Label if source register is zero
                                    // Source Register | Label Address
                                    if (Registers[(REGISTER)Fetch()] == 0) { PC = Fetch(); }
                                    else { Fetch(); }
                                    break;
                                #endregion

                                #region Move/Load Commands
                                case OPCODE.MOV: // RD <- RS
                                    // Move data from source register to destination register                                 
                                    Registers[(REGISTER)Fetch()] = Registers[(REGISTER)Fetch()];
                                    break;

                                case OPCODE.LDA:
                                    // Load the Address of the Label into the Register Destination register.
                                    // This instruction should ONLY work if the label is associated with a DIRECTIVE.
                                    // THIS command must NOT be used to get the address of an instruction.
                                    // Destination Register | Label Address
                                    Registers[(REGISTER)Fetch()] = FLAGS[Fetch(0, false)].Is(TOKEN.DirByte, TOKEN.DirInt) ? Fetch()
                                        : throw new Exception("The provided label was not associated with a directive!");

                                    break;

                                case OPCODE.LDR: // Load destination register with data from Mem
                                    switch (FetchFlag(4))
                                    {
                                        case TOKEN.Register:
                                            Registers[(REGISTER)Fetch()] = FetchAddress(Registers[(REGISTER)Fetch()]);
                                            break;

                                        case TOKEN.Label:
                                            Registers[(REGISTER)Fetch()] = FetchAddress(Fetch());
                                            break;
                                    }
                                    break;

                                case OPCODE.LDB: // Load destination register with byte from Mem
                                    switch (FetchFlag(4)) // Destination Register | Label Address
                                    {
                                        case TOKEN.Register:
                                            Registers[(REGISTER)Fetch()] = FetchByteAddress(Registers[(REGISTER)Fetch()]);
                                            break;

                                        case TOKEN.Label:
                                            Registers[(REGISTER)Fetch()] = FetchByteAddress(Fetch());
                                            break;
                                    }
                                    break;
                                #endregion

                                #region Store Commands
                                case OPCODE.STR: // Store data into Mem from source register
                                    StoreBytes(Fetch(), Fetch());
                                    break;

                                case OPCODE.STB: // Store byte into Mem from source register   
                                    StoreByte(Fetch(), Fetch());
                                    break;
                                #endregion

                                #region Arithmetic
                                case OPCODE.ADD:
                                    Registers[(REGISTER)Fetch()] += Registers[(REGISTER)Fetch()];
                                    break;

                                case OPCODE.ADI:
                                    Registers[(REGISTER)Fetch()] += Fetch();
                                    break;

                                case OPCODE.SUB:
                                    Registers[(REGISTER)Fetch()] -= Registers[(REGISTER)Fetch()];
                                    break;

                                case OPCODE.MUL:
                                    Registers[(REGISTER)Fetch()] *= Registers[(REGISTER)Fetch()];
                                    break;

                                case OPCODE.DIV:
                                    Registers[(REGISTER)Fetch()] /= Registers[(REGISTER)Fetch()];
                                    break;
                                #endregion

                                #region Logical
                                case OPCODE.AND:
                                    // not implemented
                                    break;

                                case OPCODE.OR:
                                    // not implemented
                                    break;

                                case OPCODE.CMP:
                                    Registers[(REGISTER)Fetch(0, false)] = Compare(Registers[(REGISTER)Fetch()], Registers[(REGISTER)Fetch()]);
                                    break;
                                #endregion

                                #region Trap Commands
                                case OPCODE.TRP:
                                    switch (Fetch())
                                    {
                                        case 0:
                                            done = true;
                                            break;

                                        case 1:
                                            Console.Write((int)Registers[REGISTER.R3]);
                                            break;

                                        case 2:
                                            Registers[REGISTER.R3] = int.Parse(Console.ReadKey().KeyChar.ToString());
                                            break;

                                        case 3:
                                            Console.Write((char)Registers[REGISTER.R3]);
                                            break;

                                        case 4:
                                            Registers[REGISTER.R3] = (byte)Console.ReadKey().KeyChar;
                                            break;

                                        case 99:
                                            // Doesn't do anything - set debug breakpoint here.

                                            break;
                                    }
                                    Fetch();    // Null operand pop.
                                    break;
                                #endregion

                                #region Multi-Threading
                                case OPCODE.RUN:
                                    InitializeThread();
                                    InitializeThreadRegisters(ActiveThread);
                                    PC = Fetch(); 
                                    break;

                                case OPCODE.END:
                                    TerminateThread(ActiveThread);
                           //         ContextSwitch();
                                    Fetch(); Fetch();
                                    break;

                                case OPCODE.BLK:
                                    Fetch(); Fetch();
                                    try { ThreadQueue[ActiveThread].Block(); break; } 
                                    catch(Exception) { throw new InvalidOperationException("BLK will only work when called within the Main thread!"); }
                                    //if (ActiveThread is THREAD.Main) { ThreadQueue[THREAD.Main].Block(); break; }
                                    //else throw new Exception("BLK will only work when called within the Main thread!");

                                case OPCODE.LCK:
                                    int lock_address = Fetch();
                                    int lock_value = FetchAddress(lock_address);

                                    switch (lock_value)
                                    {
                                        case int unlocked when unlocked == -1:



                                            break;

                                        case int locked when ThreadQueue.Any(t => (int)t.ID == lock_value):
                                            if ((THREAD)locked == ActiveThread)
                                            {
                                                Logger.LogWarning($"Instruction to lock an already locked mutex by {ActiveThread.GetType().Name}");
                                            }


                                            break;

                                        default:
                                            break;

                                    } Fetch();
                                    break;

                                case OPCODE.ULK:
                                    int unlock_address = Fetch();
                                    int unlock_value = FetchAddress(unlock_address);

                                    switch (unlock_value)
                                    {
                                        case int unlocked when unlocked == -1:



                                            break;

                                        case int locked when ThreadQueue.Any(t => (int)t.ID == unlock_value):
                                            if ((THREAD)locked == ActiveThread)
                                            {
                                                Logger.LogWarning($"Instruction to lock an already locked mutex by {ActiveThread.GetType().Name}");
                                            }


                                            break;

                                        default:
                                            break;

                                    }
                                    Fetch();
                                    break;
                                    #endregion
                            }
                            break;

                        case TOKEN.Int:
                        case TOKEN.Label:
                        case TOKEN.Register:
                        case TOKEN.Char:
                        case TOKEN.Null:
                            PC -= 1;
                            break;
                        default:
                            throw new Exception();
                    }
                    ContextSwitch();
                }
                catch (Exception e)
                {
                    Logger.Log($"Error encountered on tick: [{ticks}]" +
                        $"\n\tToken: [{FetchFlag(-3)}]" +
                        $"\n\tFetched: [{Fetch(-3, false)}] {(OPCODE)Fetch(-3, false)}" +
                        $"\n\tOperand_A: [{Fetch(-2, false)}]\t{FetchFlag(-2)}" +
                        $"\n\tOperand_B: [{Fetch(-1, false)}]\t{FetchFlag(-1)}" +
                        $"\n{e.Message}.", LOG.EXCEPTION);
                    // Program is still running.
                }
            }
        }
    }
}
