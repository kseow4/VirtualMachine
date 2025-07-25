using System;
using System.Collections.Generic;
using System.Linq;
using VirtualMachine.Enumerations;

namespace VirtualMachine.Depreciated
{
    public class Depreciated_VirtualMachine
    {
        public Dictionary<string, int> SymbolTable = new Dictionary<string, int>(); // <label, address offset>
        public Dictionary<REGISTER, Int32> Registers = Enum.GetValues(typeof(REGISTER)).Cast<REGISTER>().ToList().Zip(Enumerable.Repeat(new Int32(), Enum.GetValues(typeof(REGISTER)).Length), (r, v) => new { r, v }).ToDictionary(i => i.r, i => i.v);
        public Int32 PC { get => Registers[REGISTER.PC]; set => Registers[REGISTER.PC] = value; }
        public readonly TOKEN[] FLAGS = new TOKEN[MEMORY_SIZE];
        public readonly byte[][] MEMORY = Enumerable.Repeat(new byte[ADDRESS_SIZE], MEMORY_SIZE).ToArray();
        private readonly List<string> Exceptions = new List<string>();
        public Depreciated_Assembler Depreciated_Assembler;

        public static int MEMORY_SIZE { get; } = 4000/*1024*/;
        public static int ADDRESS_SIZE { get; } = 4;
        public static int CHUNK { get; } = 4;

        public Depreciated_VirtualMachine(params string[] args)
        {
            Depreciated_Assembler = new Depreciated_Assembler(this);
            Depreciated_Assembler.Run(args);
            Run();
        }

        private long ticks = 0;
        private int Fetch(int location = 0, bool increment = true) => BitConverter.ToInt32(MEMORY[location + (increment ? PC++ : PC)], 0);
        private int FetchAddress(int location) => BitConverter.ToInt32(MEMORY[location], 0);
        private byte[] Store(int value) { byte[] bytes = BitConverter.GetBytes(value); Array.Resize(ref bytes, 4); return bytes; }
        private int Compare(int destination, int source) => destination > source ? int.MaxValue : destination < source ? int.MinValue : 0;
        private TOKEN FetchFlag(int offset = 0) => FLAGS[PC + offset];
        public void Run()
        {
            PC = 0;
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
                                    if (Registers[(REGISTER)Fetch()] > 0) { PC = Fetch(); }
                                    else { Fetch(); }

                                    break;

                                case OPCODE.BRZ:
                                    // Branch to Label if source register is zero
                                    // Source Register | Label Address
                                    if (Registers[(REGISTER)Fetch()] == 0) { PC = Fetch(); }
                                    else { Fetch(); }
                                    break;
                                #endregion

                                #region Move-Load-Store Commands
                                case OPCODE.MOV:
                                    // Move data from source register to destination register                                 
                                    Registers[(REGISTER)Fetch()] = Registers[(REGISTER)Fetch()];
                                    break;

                                case OPCODE.LDA:
                                    // Load the Address of the Label into the Register Destination register.
                                    // This instruction should ONLY work if the label is associated with a DIRECTIVE.
                                    // THIS command must NOT be used to get the address of an instruction.
                                    // Destination Register | Label Address
                                    Registers[(REGISTER)Fetch()] = FLAGS[Fetch(0, false)] == TOKEN.DirChar || FLAGS[Fetch(0, false)] == TOKEN.DirInt ? Fetch() : throw new Exception();
                                    break;

                                case OPCODE.LDR: // Load destination register with data from Mem
                                    switch (FetchFlag(1))
                                    {
                                        case TOKEN.Register:
                                            int destinationRegister = Fetch();
                                            //switch (FLAGS[Registers[(REGISTER)Fetch(1, false)]])
                                            //{
                                            //    case TOKEN.DirChar:
                                            //        try
                                            //        {

                                            //        }
                                            //        catch()
                                            //        {

                                            //        }
                                            //        break;
                                            //}
                                            Registers[(REGISTER)Fetch()] = FetchAddress(Registers[(REGISTER)Fetch()]);

                                            int dest = Fetch();
                                            var ff = FetchFlag();
                                            var fet = FetchAddress(Registers[(REGISTER)dest]);
                                            int src = Fetch();
                                            var aa = FetchFlag();
                                            var far = FetchAddress(Registers[(REGISTER)src]);
                                            var fgff = FLAGS[Registers[(REGISTER)src]];
                                            // Registers[(REGISTER)Fetch()] = FetchAddress(Registers[(REGISTER)Fetch()]);


                                            break;

                                        case TOKEN.Label:
                                            Registers[(REGISTER)Fetch()] = FetchAddress(Fetch());
                                            break;

                                        default:
                                            throw new Exception();

                                    }
                                    break;

                                case OPCODE.LDB: // Load destination register with byte from Mem
                                    // Destination Register | Label Address
                                    switch (FetchFlag(1))
                                    {
                                        case TOKEN.Register:
                                            Registers[(REGISTER)Fetch()] = FetchAddress(Registers[(REGISTER)Fetch()]);
                                            break;

                                        case TOKEN.Label:
                                            Registers[(REGISTER)Fetch()] = FetchAddress(Fetch());
                                            break;

                                        default:
                                            throw new Exception();
                                    }
                                    break;

                                case OPCODE.STR: // Store data into Mem from source register
                                                 // TRY TO LOAD 4.


                                case OPCODE.STB: // Store byte into Mem from source register                                
                                    int source = Fetch(); // Source Register
                                    switch (FetchFlag())
                                    {
                                        case TOKEN.Register:
                                            MEMORY[Registers[(REGISTER)Fetch()]] = Store(Registers[(REGISTER)source]);
                                            //      FLAGS[Registers[(REGISTER)Fetch(-1, false)]] = FetchFlag(source);
                                            break;

                                        case TOKEN.Label:
                                            MEMORY[Fetch()] = Store(Registers[(REGISTER)source]);
                                            //       FLAGS[Registers[(REGISTER)Fetch(-1, false)]] = FetchFlag(source);
                                            break;

                                        default:
                                            throw new Exception();
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

                                #endregion



                                case OPCODE.CMP:
                                    Registers[(REGISTER)Fetch()] = Compare(Registers[(REGISTER)Fetch(-1, false)], Registers[(REGISTER)Fetch()]);
                                    break;

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
                                            // NOT IMPLEMENTED YET
                                            break;

                                        case 3:
                                            Console.Write((char)Registers[REGISTER.R3]);
                                            break;

                                        case 4:
                                            // NOT IMPLEMENTED YET
                                            break;
                                    }
                                    Fetch();    // Null operand pop.
                                    break;
                                    #endregion
                            }

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

    }
}
