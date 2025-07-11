using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VirtualMachine.Enumerations;
using static System.StringSplitOptions;
using static VirtualMachine.Enumerations.DIRECTIVE;

namespace VirtualMachine.Depreciated
{
    public class Depreciated_Assembler
    {
        private int Counter = 0;
        private int CodeCounter = 0;
        private int DirCounter = Depreciated_VirtualMachine.MEMORY_SIZE;
        private List<string> ExceptionList = new List<string>();
        private static Depreciated_VirtualMachine Depreciated_VirtualMachine;
        private static readonly char[] splitters = new char[] { '\t', ' ' };
        private static readonly char[] newLiners = new char[] { '\r', '\n' };
        private static readonly char[] commenters = new char[] { ';', '#' };
        private static readonly Regex asmRegex = new Regex(@"(String)?(((OPCODE)(REGISTER|Int32|Byte|Char|String){1,2})|((DIRECTIVE)(Int32|byte)?)){1}");
        // private static readonly Regex asmRegex = new Regex(@"(String)?(((OPCODE)(REGISTER|Int32|Byte|Char|String){1,2}))|((DIRECTIVE)(Int32|byte)?){1}");
        public Depreciated_Assembler(Depreciated_VirtualMachine vm) => Depreciated_VirtualMachine = vm;

        public void Run(params string[] asmFiles)
        {
            foreach (string asmFile in asmFiles)
            {
                //// FIRST PASS
                try { FirstPass(Tokenizer(ReadInAsm(asmFile))); }
                catch (Exception firstpassException) { PrintExceptions(firstpassException); }

                //// SECOND PASS
                try { SecondPass(Tokenizer(ReadInAsm(asmFile))); }
                catch (Exception secondpassException) { PrintExceptions(secondpassException); }
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

            foreach (List<object> tokenTypes in tokenizedLists)
            {
                try
                {
                    listIndex++;
                    object token = tokenTypes.First();
                    byte[] bytes;
                    switch (token)
                    {
                        case char c when token is char:
                        case byte b when token is byte:
                        case string s when token is string:
                            if (tokenTypes.Count < 2) { ExceptionList.Add($"Label without instruction: \"{string.Join(" ", tokenTypes)}\" on line: [{listIndex}]"); }
                            if (Depreciated_VirtualMachine.SymbolTable.ContainsKey(token.ToString())) { ExceptionList.Add($"Duplicate Label: \"{token.ToString()}\" on line: [{listIndex}]"); }
                            switch (tokenTypes[1])
                            {
                                case OPCODE op when tokenTypes[1] is OPCODE:
                                    Depreciated_VirtualMachine.SymbolTable.Add(token.ToString(), CodeCounter++ * 3);

                                    break;

                                case DIRECTIVE dir when tokenTypes[1] is DIRECTIVE:
                                    Depreciated_VirtualMachine.SymbolTable.Add(token.ToString(), --DirCounter);
                                    switch (dir)
                                    {
                                        case INT:
                                            bytes = BitConverter.GetBytes((int)tokenTypes[2]);
                                            Array.Resize(ref bytes, 4);
                                            Depreciated_VirtualMachine.FLAGS[DirCounter] = TOKEN.DirInt;
                                            Depreciated_VirtualMachine.MEMORY[DirCounter] = bytes;
                                            break;

                                        case BYT:
                                            bytes = BitConverter.GetBytes((char)tokenTypes[2]);
                                            Array.Resize(ref bytes, 4);
                                            Depreciated_VirtualMachine.FLAGS[DirCounter] = TOKEN.DirChar;
                                            Depreciated_VirtualMachine.MEMORY[DirCounter] = bytes;
                                            break;
                                    }
                                    break;
                            }
                            break;

                        case OPCODE op when token is OPCODE:
                            CodeCounter++;
                            break;

                        case DIRECTIVE dir when token is DIRECTIVE:
                            switch (dir)
                            {
                                case INT:
                                    bytes = BitConverter.GetBytes((int)tokenTypes[1]);
                                    Array.Resize(ref bytes, 4);
                                    Depreciated_VirtualMachine.MEMORY[--DirCounter] = bytes;
                                    Depreciated_VirtualMachine.FLAGS[DirCounter] = TOKEN.DirInt;
                                    break;

                                case BYT:
                                    bytes = BitConverter.GetBytes((char)tokenTypes[1]);
                                    Array.Resize(ref bytes, 4);
                                    Depreciated_VirtualMachine.MEMORY[--DirCounter] = bytes;
                                    Depreciated_VirtualMachine.FLAGS[DirCounter] = TOKEN.DirChar;
                                    break;
                            }
                            break;

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
                        byte[] bytes;
                        if (token is string && token == tokens.First()) { continue; }
                        switch (token)
                        {
                            case string s when token is string &&
                            Depreciated_VirtualMachine.SymbolTable.TryGetValue(s, out int address):
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Label;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = BitConverter.GetBytes(address);
                                break;

                            case OPCODE op when token is OPCODE:
                                bytes = BitConverter.GetBytes((int)op);
                                Array.Resize(ref bytes, 4);
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Opcode;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = bytes;
                                break;

                            case DIRECTIVE dir when token is DIRECTIVE:
                                switch (dir)
                                {
                                    case INT:
                                        bytes = BitConverter.GetBytes((int)tokens[++i]);
                                        Array.Resize(ref bytes, 4);
                                        break;

                                    case BYT:
                                        bytes = BitConverter.GetBytes((char)tokens[++i]);
                                        Array.Resize(ref bytes, 4);
                                        break;
                                }
                                break;

                            case TOKEN t when token is TOKEN:
                                Depreciated_VirtualMachine.FLAGS[Counter++] = t;
                                break;

                            case REGISTER r when token is REGISTER:
                                bytes = BitConverter.GetBytes((int)r);
                                Array.Resize(ref bytes, 4);
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Register;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = bytes;
                                break;

                            case char c when token is char &&
                            Depreciated_VirtualMachine.SymbolTable.TryGetValue(c.ToString(), out int address):
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Label;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = BitConverter.GetBytes(address);
                                break;

                            case char c when token is char:
                                bytes = BitConverter.GetBytes(c);
                                Array.Resize(ref bytes, 4);
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Char;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = bytes;
                                break;

                            case byte b when token is byte:
                                bytes = BitConverter.GetBytes(b);
                                Array.Resize(ref bytes, 4);
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Byte;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = bytes;
                                break;

                            case Int32 ii when token is Int32:
                                bytes = BitConverter.GetBytes(ii);
                                Array.Resize(ref bytes, 4);
                                Depreciated_VirtualMachine.FLAGS[Counter] = TOKEN.Int;
                                Depreciated_VirtualMachine.MEMORY[Counter++] = bytes;
                                break;

                            default:
                                throw new ArgumentException($"{token}");
                        }
                    }
                    catch (Exception e) { ExceptionList.Add(e.Message + "\t" + $"[{listIndex}] " + token.ToString() + ":(" + tokens[i] + ")\t" + "Error loading data into bytecode/memory."); }
                }
            }
            if (ExceptionList.Count > 0) { throw new Exception($"Error Count in Second Pass: [{ExceptionList.Count}]"); }
        }
        private void CheckForErrors<T>(List<List<T>> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                if (CheckTokens(lists[i])) { ExceptionList.Add($"Error occurred on line [{i}]:\t" + string.Join("\t", lists[i])); }
            }
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
                        else if (token.StartsWith("."))
                        {
                            if (Enum.IsDefined(typeof(DIRECTIVE), token.Substring(1)))
                            {
                                Enum.TryParse(token.Substring(1), true, out DIRECTIVE directive);
                                tokenTypes.Add(directive);
                            }
                        }
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

        public bool CheckTokens<T>(List<T> tokens) => CheckTcount(tokens) && CheckTpattern(tokens);

    }
}
