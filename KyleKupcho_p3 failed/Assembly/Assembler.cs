using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VirtualMachine.Enumerations;
using static System.StringSplitOptions;
using static VirtualMachine.Enumerations.DIRECTIVE;

namespace VirtualMachine.Assembly
{
    public class Assembler
    {
        private int Counter = 0;
        private int CodeCounter = 0;
        private int DirCounter = 0;
        private int BYT_Counter = 0;
        private int INT_Counter = 0;
        private int LBL_Counter = 0;

        private readonly List<string> ExceptionList = new List<string>();

        private static readonly int INTEGER_SIZE = 4;
        private static readonly int INSTRUCTION_SIZE = 12;
        private static VirtualMachine VirtualMachine;
        private static readonly char[] splitters = new char[] { '\t', ' ' };
        private static readonly char[] newLiners = new char[] { '\r', '\n' };
        private static readonly char[] commenters = new char[] { ';', '#' };
        private static readonly Regex asmRegex = new Regex(@"(String)?(((OPCODE)(REGISTER|Int32|Byte|Char|String){1,2})|((DIRECTIVE)(Int32|byte)?)){1}");

        public Assembler(VirtualMachine vm) => VirtualMachine = vm;

        public void Run(params string[] asmFiles)
        {
            foreach (string asmFile in asmFiles)
            {
                // FIRST PASS
                try { FirstPass(Tokenizer(ReadInAsm(asmFile))); }
                catch (Exception fp) { PrintExceptions(fp); }

                //SECOND PASS
                try { SecondPass(Tokenizer(ReadInAsm(asmFile))); }
                catch (Exception sp) { PrintExceptions(sp); }
            }
        }

        public List<List<string>> ReadInAsm(string asm)
        {
            List<string> asmLines = File.ReadAllText(asm, Encoding.ASCII).Split(newLiners, RemoveEmptyEntries).ToList();
            List<List<string>> lineTokens = new List<List<string>>();

            foreach (string line in asmLines)
            {
                lineTokens.Add(line.Substring(0, line.IndexOfAny(commenters) < 0 ? line.Length : line.IndexOfAny(commenters)).Split(splitters, RemoveEmptyEntries).ToList());
            }
            CheckForErrors(lineTokens);

            return lineTokens;
        } 

        public void FirstPass(List<List<object>> tokenizedLists)
        {
            int listIndex = 0;

            DirCounter = VirtualMachine.SL = CalculateDirectiveOffset(tokenizedLists);

            foreach (List<object> tokenTypes in tokenizedLists)
            {
                try
                {
                    listIndex++;
                    int tokenIterator = 0;
                    object token = tokenTypes[tokenIterator++];
                    switch (token)
                    {
                        case char c when token is char:
                        case byte b when token is byte:
                        case string s when token is string:
                            if (tokenTypes.Count < 2) { ExceptionList.Add($"Label without instruction: \"{string.Join(" ", tokenTypes)}\" on line: [{listIndex}]"); }
                            if (VirtualMachine.SymbolTable.ContainsKey(token.ToString())) { ExceptionList.Add($"Duplicate Label: \"{token}\" on line: [{listIndex}]"); }
                            switch (tokenTypes[tokenIterator])
                            {
                                case OPCODE op when tokenTypes[tokenIterator] is OPCODE:
                                    VirtualMachine.SymbolTable.Add(token.ToString(), CodeCounter++ * INSTRUCTION_SIZE);
                                    break;

                                case DIRECTIVE dir when tokenTypes[tokenIterator++] is DIRECTIVE:
                                    VirtualMachine.SymbolTable.Add(token.ToString(), DirCounter);
                                    if (dir is INT) goto Int; else goto Byt;
                            }
                            break;

                        case OPCODE op when token is OPCODE:
                            CodeCounter++;
                            break;
                            
                        case DIRECTIVE dir when token is DIRECTIVE:
                            if (dir is INT) goto Int; else goto Byt;
                            Byt: InsertDirective((char)(tokenTypes.Count > tokenIterator ? tokenTypes[tokenIterator] : '\0')); break;
                            Int: InsertDirective((int)(tokenTypes.Count > tokenIterator ? tokenTypes[tokenIterator] : 0)); break;

                        default:
                            ExceptionList.Add($"{token} is {(token is null ? "null" : "invalid type")}: ");
                            break;
                    }
                }
                catch (Exception e) { ExceptionList.Add($"Error encountered on Line: [{listIndex}]: " + e.Message + $" [CodeCounter:{CodeCounter}, DirCounter:{DirCounter}]"); }
            }
            if (ExceptionList.Count > 0) { throw new Exception($"Error Count in First Pass: [{ExceptionList.Count}]"); }
        }

        public void SecondPass(List<List<object>> tokenizedLists)
        {
            int listIndex = 0;

            foreach (List<object> tokens in tokenizedLists)
            {
                listIndex++;

                for (int i = 0; i < tokens.Count; i++)
                {
                    object token = tokens[i];
                    try
                    {
                        if (token is string && token == tokens.First()) { continue; }
                        switch (token)
                        {
                            case DIRECTIVE dir when token is DIRECTIVE: ++i; break;
                           
                            case TOKEN t when token is TOKEN: foreach (byte bite in BitConverter.GetBytes((int)t)) { InsertToken(t, bite); } break;

                            case OPCODE op when token is OPCODE: foreach (byte bite in BitConverter.GetBytes((int)op)) { InsertToken(TOKEN.Opcode, bite); } break;

                            case REGISTER r when token is REGISTER: foreach (byte bite in BitConverter.GetBytes((int)r)) { InsertToken(TOKEN.Register, bite); } break;

                            case Int32 ii when token is Int32: foreach (byte bite in BitConverter.GetBytes(ii)) { InsertToken(TOKEN.Int, bite); } break;

                            case string s when token is string && VirtualMachine.SymbolTable.TryGetValue(s, out int address):
                                foreach (byte bite in BitConverter.GetBytes(address)) { InsertToken(TOKEN.Label, bite); } break;

                            case char c when token is char && VirtualMachine.SymbolTable.TryGetValue(c.ToString(), out int address):
                                foreach (byte bite in BitConverter.GetBytes(address)) { InsertToken(TOKEN.Label, bite); } break;

                            case char c when token is char: InsertToken(TOKEN.Char, Convert.ToByte(c)); break;
                            
                            case byte b when token is byte: InsertToken(TOKEN.Byte, b); break;

                            default:
                                throw new ArgumentException($"{token}");
                        }
                    }
                    catch (Exception e) { ExceptionList.Add(e.Message + "\t" + $"[{listIndex}] " + token.ToString() + ":(" + tokens[i] + ")\t" + "Error loading data into bytecode/memory."); }
                }
            }
            if (ExceptionList.Count > 0) { throw new Exception($"Error Count in Second Pass: [{ExceptionList.Count}]"); }
        }

        private void InsertToken(TOKEN token, byte bite) { VirtualMachine.FLAGS[Counter] = token; VirtualMachine.MEMORY[Counter++] = bite; }

        private void InsertDirective(int token) 
        { 
            foreach (byte bite in BitConverter.GetBytes(token))
            {
                VirtualMachine.FLAGS[DirCounter] = TOKEN.DirInt;
                VirtualMachine.MEMORY[DirCounter--] = bite;
            } 
        }

        private void InsertDirective(char token)
        {
            VirtualMachine.FLAGS[DirCounter] = TOKEN.DirByte;
            VirtualMachine.MEMORY[DirCounter--] = Convert.ToByte(token);
        }

        private int CalculateDirectiveOffset(List<List<object>> tokenizedLists) => ((tokenizedLists.Count + 1 - (BYT_Counter + INT_Counter)) * INSTRUCTION_SIZE) + (INT_Counter * INTEGER_SIZE) + BYT_Counter + LBL_Counter;

        private void CheckForErrors<T>(List<List<T>> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                if (CheckTokens(lists[i])) { ExceptionList.Add($"Error occurred on line [{i}]:\t" + string.Join("\t", lists[i])); }
            }
        }

        private void PrintExceptions(Exception e)
        {
            int errorCount = 0;
            Console.WriteLine("ERRORS HAVE BEEN DETECTED:");
            Console.WriteLine("\t" + e.Message);
            foreach (string exception in ExceptionList) { Console.WriteLine($"\t{++errorCount}. {exception}"); }
            Console.WriteLine();
        }

        private List<List<object>> Tokenizer(List<List<string>> lines)
        {
            List<List<object>> tokenLists = new List<List<object>>();
            foreach (List<string> tokens in lines)
            {
                if (tokens.Contains("TRP") || tokens.Contains("JMP") || tokens.Contains("JMR")) { tokens.Add("Null"); }
                if (tokens.Count > 3 && tokens[2] == "\'" && tokens[3] == "\'") { tokens.RemoveAt(3); tokens[2] = " "; }
                try
                {
                    List<object> tokenTypes = new List<object>();
                    foreach (string token in tokens)
                    {
                        if (Enum.IsDefined(typeof(OPCODE), token)) { Enum.TryParse(token, true, out OPCODE opcode); tokenTypes.Add(opcode); }
                        else if (Enum.IsDefined(typeof(REGISTER), token)) { Enum.TryParse(token, true, out REGISTER register); tokenTypes.Add(register); }
                        else if (token.StartsWith(".")) { if (Enum.IsDefined(typeof(DIRECTIVE), token.Substring(1))) 
                            {
                                Enum.TryParse(token.Substring(1), true, out DIRECTIVE directive);
                                tokenTypes.Add(directive); _ = directive == INT ? INT_Counter++ : BYT_Counter++; 
                                if (tokens.Count > 2) { LBL_Counter++; }
                            } }
                        else
                        {
                            if (token == "Null") { tokenTypes.Add(TOKEN.Null); }
                            else if (Int32.TryParse(token, out Int32 integer)) { tokenTypes.Add(integer); }
                            else if (char.TryParse(token.Trim('\'', '\"'), out char character)) { tokenTypes.Add(character); }
                            else if (byte.TryParse(token.Trim('\'', '\"'), out byte bite)) { tokenTypes.Add(bite); }
                            else 
                            {
                                if (token == "'\\n'" || token == "'\\r'")
                                {
                                    tokenTypes.Add('\n');
                                }
                                else
                                {
                                    tokenTypes.Add(token);
                                }
                            }
                        }
                    }
                    if (tokenTypes.Count > 0) { tokenLists.Add(tokenTypes); }
                }
                catch (Exception e)
                {
                    ExceptionList.Add($"Error occurred in Tokenizer: [{string.Join(" ", tokens)}] => {e.Message}");
                }
            }
            return tokenLists;
        }

        private bool CheckTcount<T>(List<T> tokens) => tokens.Count < 4 && tokens.Count > 0;

        private bool CheckTpattern<T>(List<T> tokens) => asmRegex.IsMatch(string.Join("", tokens.Select(t => t.GetType().Name)));

        private bool CheckTokens<T>(List<T> tokens) => CheckTcount(tokens) && CheckTpattern(tokens);
    }
}