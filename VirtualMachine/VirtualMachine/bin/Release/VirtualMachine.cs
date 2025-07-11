using System;
using System.Collections.Generic;
using System.Linq;
using VirtualMachine.Assembly;
using VirtualMachine.Enumerations;
using VirtualMachine.Threading;
using VirtualMachine.VMTools;
using VirtualMachine.VMTools.States;
using static VirtualMachine.VMTools.VM_Extensions;

namespace VirtualMachine
{
    public class VirtualMachine
    {
        private long ticks = 0;

        public Assembler Assembler;

        public ThreadQueue ThreadQueue = new ThreadQueue();

        //  public static ToolFactory Tools = new ToolFactory();

        public readonly List<string> Exceptions = new List<string>();

        public Dictionary<string, int> SymbolTable = new Dictionary<string, int>(); // <label, address offset>

        public Dictionary<REGISTER, Int32> Registers = Enum.GetValues(typeof(REGISTER)).Cast<REGISTER>().ToList().Zip(Enumerable.Repeat(new Int32(), Enum.GetValues(typeof(REGISTER)).Length), (r, v) => new { r, v }).ToDictionary(i => i.r, i => i.v);


        public Int32 PC { get => Registers[REGISTER.PC]; set => Registers[REGISTER.PC] = value; }
        public Int32 SL { get => Registers[REGISTER.SL]; set => Registers[REGISTER.SL] = value; }
        public Int32 SB { get => Registers[REGISTER.SB]; set => Registers[REGISTER.SB] = value; }
        public Int32 SP { get => Registers[REGISTER.SP]; set => Registers[REGISTER.SP] = value; }
        public Int32 FP { get => Registers[REGISTER.FP]; set => Registers[REGISTER.FP] = value; }
        public THREAD ActiveThread { get; set; }
        public int RegistersBlock { get => 4 * Registers.Count; }


        public static int MEMORY_SIZE { get; } = 500000;

        public static int STACK_SIZE { get; } = 12000;

        public static int MAX_THREAD { get; internal set; }

        public static int CHUNK { get; } = 4;


        public readonly TOKEN[] FLAGS = new TOKEN[MEMORY_SIZE];

        public readonly byte[] MEMORY = new byte[MEMORY_SIZE];

        public VirtualMachine(params string[] args)
        {
            Assembler = new Assembler(this);
            Assembler.Run(args ?? new string[] {"Resources/proj4.asm"});
            Run();
        }

        private int Compare(int destination, int source) => destination > source ? int.MaxValue : destination < source ? int.MinValue : 0;
        private int Fetch(int offset = 0, bool increment = true) => BitConverter.ToInt32(MEMORY.Skip(offset + (increment ? (PC += 4) - 4 : PC)).Take(CHUNK).ToArray(), 0);
        private int FetchAddress(int location) => BitConverter.ToInt32(MEMORY.Skip(location - 3).Take(CHUNK).Reverse().ToArray(), 0);
        private int FetchByteAddress(int location) => MEMORY[location];
        private TOKEN FetchFlag(int offset = 0) => FLAGS[PC + offset];
        private byte[] StoreInt(int value) => BitConverter.GetBytes(value);
        private THREAD NextAvailableThread() => ThreadQueue.FirstOrDefault(t => t.State is Inactive).ID;
        private int StackOffset(THREAD thread) => MEMORY_SIZE - (int)thread * STACK_SIZE;
        private void StoreRegister(REGISTER register, int location) => StoreInt(Registers[register]).ToList().ForEach(bite => MEMORY[location--] = bite);
        private void StoreRegister(Int32 register, int location) => StoreInt(register).ToList().ForEach(bite => MEMORY[location--] = bite);
        private void LoadRegister(Int32 register, int location) => register = FetchAddress(location);
        private void LoadRegister(REGISTER register, int location) => Registers[register] = FetchAddress(location);

        private void TerminateThread(THREAD thread) => ThreadQueue[thread != THREAD.Main ? thread : throw new Exception("Cannot invoke END instruction on Main Thread!")].Deactivate();

        private THREAD NextActiveThread(THREAD thread)
        {
            THREAD end = Enum.GetValues(typeof(THREAD)).Cast<THREAD>().Last();
            THREAD index = (thread == end) ? 0 : ActiveThread + 1;
            for (int i = 0; i < ThreadQueue.Count(); i++)
            {
                if (ThreadQueue[index].State is Active) return index;
                index = (index == end) ? 0 : index++;
            }
            return thread;
        }

        private void InitializeThread()
        {
            try
            {
                ThreadQueue[ActiveThread = NextAvailableThread()].Activate();


            }
            catch (Exception) { throw new StackOverflowException("No available threads!"); }

        }

        private void StoreThreadState(THREAD thread)
        {
            int memloc = StackOffset(thread) - 4;
            foreach (KeyValuePair<REGISTER, Int32> register in Registers) { StoreRegister(register.Value, memloc); memloc -= 4; }

            //StoreRegister(PC, memloc); memloc -= 4;
            //Registers.Take(8).ToList().ForEach(register => { StoreRegister(register.Key, memloc); memloc -= 4; });
            //StoreRegister(SP, memloc); memloc -= 4;
            //StoreRegister(FP, memloc); memloc -= 4;
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
            if (next is THREAD.Main && ThreadQueue[next].Blocked) next = NextActiveThread(next);
            StoreThreadState(ActiveThread);
            ActiveThread = next;
            LoadThreadState(ActiveThread);
        }

        public void Run()
        {
            PC = 0;
            //  SL = MEMORY_SIZE - STACK_SIZE - 4;
            //  FP = SP = SB = MEMORY_SIZE - 4 - RegistersBlock;
            // SL = MEMORY_SIZE - STACK_SIZE - 4;
            SB = MEMORY_SIZE;
            FP = SP = SB - 4;
            InitializeThread();
            StoreThreadState(ActiveThread);


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
                                    int registerLocation = Fetch(); // Register with the data to input to memory
                                    int memoryLocation = Fetch();
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
                                    break;

                                case OPCODE.STB: // Store byte into Mem from source register                                
                                    int destination = Fetch(); // Source Register
                                    int source = Fetch();
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
                                    Fetch();
                                    InitializeThread();
                                    PC = Fetch();
                                    break;

                                case OPCODE.END:
                                    TerminateThread(ActiveThread);
                                    ContextSwitch();
                                    Fetch(); Fetch();
                                    break;

                                case OPCODE.BLK:
                                    Fetch(); Fetch();
                                    if (ActiveThread is THREAD.Main) { ThreadQueue[THREAD.Main].Block(); break; }
                                    else throw new Exception("BLK will only work when called within the Main thread!");

                                case OPCODE.LCK:

                                    break;

                                case OPCODE.ULK:

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

                }
                catch (Exception e)
                {
                    Exceptions.Add($"Error encountered on tick: [{ticks}]" +
                        $"\n\tToken: [{FetchFlag(-3)}]" +
                        $"\n\tFetched: [{Fetch(-3, false)}] {(OPCODE)Fetch(-3, false)}" +
                        $"\n\tOperand_A: [{Fetch(-2, false)}]\t{FetchFlag(-2)}" +
                        $"\n\tOperand_B: [{Fetch(-1, false)}]\t{FetchFlag(-1)}" +
                        $"\n{e.Message}.");
                    // Program is still running.
                }
            }
        }

        public void GetLogs()
        {
            foreach (string item in Exceptions)
            {
                Console.WriteLine(item);

            }
        }
    }
}
