namespace VirtualMachine.Enumerations
{
    /// <summary>
    /// Token Types
    /// </summary>
    public enum TOKEN
    {
        Null,
        Opcode,
        Register,
        Label,
        Immediate,
        Char,
        Int,
        Byte,
        /// <summary>
        /// .INT Directive
        /// </summary>
        DirInt,
        /// <summary>
        /// .BYT Directive
        /// </summary>
        DirChar,
        /// <summary>
        /// .BYT Directive
        /// </summary>
        DirByte,
        Directive,
    }
}
