using System.ComponentModel;

namespace VirtualMachine.Enumerations
{
    /// <summary>
    /// Operation Code
    /// </summary>
    [Description("Operation Code")]
    public enum OPCODE
    {
        #region None Instructions
        /// <summary>
        /// NONE Instruction: Does nothing.
        /// </summary>
        [Description("None Instruction: Does nothing.")]
        NONE = 0,
        #endregion

        #region Jump Instructions
        /// <summary>
        /// Jumps to the address of the specified label.
        /// </summary>
        [Description("Jump Instruction: Jumps to the address of specified label."), Category("Special Operations")]
        JMP = 1,

        /// <summary>
        /// Jumps to the address specified by the value contained in the given register.
        /// </summary>
        [Description("Jump Instruction: Jumps to the address specified by the value contained in the given register."), Category("Special Operations")]
        JMR = 2,
        #endregion

        #region Branch Instructions
        /// <summary>
        /// Branches to the address of the specified label IF the value in the given register is NOT zero. 
        /// </summary>
        [Description("Branch Instruction: Branches to the address of the specified label IF the value in the given register is NOT zero.")]
        BNZ = 3,

        /// <summary>
        /// Branches to the address of the specified label IF the value in the given register is GREATER-THAN zero.
        /// </summary>
        [Description("Branch Instruction: Branches to the address of the specified label IF the value in the given register is GREATER-THAN zero.")]
        BGT = 4,

        /// <summary>
        /// Branches to the address of the specified label IF the value in the given register is LESS-THAN zero.
        /// </summary>
        [Description("Branch Instruction: Branches to the address of the specified label IF the value in the given register is LESS-THAN zero.")]
        BLT = 5,

        /// <summary>
        /// Branches to the address of the specified label IF the value in the given register is zero.
        /// </summary>
        [Description("Branch Instruction: Branches to the address of the specified label IF the value in the given register is zero.")]
        BRZ = 6,
        #endregion

        #region Move/Load Instructions
        /// <summary>
        /// Replaces the value in the left-most register with the value in the right-most register.
        /// </summary>
        [Description("Move/Load Instruction: Replaces the value in the left-most register with the value in the right-most register.")]
        MOV = 7,

        /// <summary>
        /// Loads the address of the specified label into the designated register. This instruction will only work if the label is associated with a <seealso cref="TOKEN"/> prefaced with "Dir":
        /// <list type="bullet">
        ///     <item> <see cref="TOKEN.DirInt"/> </item>
        ///     <item> <see cref="TOKEN.DirByte"/> </item>
        ///     <item> <see cref="TOKEN.DirChar"/> </item>
        /// </list>
        /// </summary>
        [Description("Move/Load Instruction: Loads the address of the specified label into the designated register.")]
        LDA = 8,

        /// <summary>
        /// Instruction Variations:
        /// <list type="bullet">
        ///     <item> 
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Register"/> </term> 
        ///         <description> Loads the preceeding 4 bytes starting at the address indicated by the value within the given register and stores the 4-bytes as an integer in the value of the specified register. </description>
        ///     </item>
        ///     <item>
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Label"/> </term>
        ///         <description> Loads the preceeding 4 bytes starting at the address of the given label and stores the 4-bytes as an integer in the value of the specified register. </description>
        ///     </item>
        /// </list>
        /// </summary>
        [Description("Move/Load Instruction: Loads the preceeding 4 bytes starting at the address indicated by the value within the given register and stores the 4-bytes as an integer in the value of the specified register.")]
        LDR = 10,

        /// <summary>
        /// Instruction Variations:
        /// <list type="bullet">
        ///     <item> 
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Register"/> </term> 
        ///         <description> Loads the byte value stored in the address indicated by the value within the given register and stores the byte in the value of the specified register. </description>
        ///     </item>
        ///     <item>
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Label"/> </term>
        ///         <description> Loads the byte value stored in the address of the given label into the specified register. </description>
        ///     </item>
        /// </list>
        /// </summary>
        [Description("Move/Load Instruction: Loads the byte value stored in the address indicated by the value within the given register and stores the byte in the value of the specified register.")]
        LDB = 12,
        #endregion

        #region Store Instructions
        /// <summary>
        /// Instruction Variations:
        /// <list type="bullet">
        ///     <item> 
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Register"/> </term> 
        ///         <description> Converts the value in the specified register into a 4-byte array and stores them sequentially starting from the address indicated by the value of the given register. </description>
        ///     </item>
        ///     <item>
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Label"/> </term>
        ///         <description> Converts the value in the specified register into a 4-byte array and stores them sequentially starting from the address of given label. </description>
        ///     </item>
        /// </list>
        /// </summary>
        [Description("Store Instruction: Converts the value in the specified register into a 4-byte array and stores them sequentially starting from the address indicated by the value of the given register.")]
        STR = 9,

        /// <summary>
        /// Instruction Variations:
        /// <list type="bullet">
        ///     <item> 
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Register"/> </term> 
        ///         <description> Stores byte value from the specified register into the address indicated by the value of the given register. </description>
        ///     </item>
        ///     <item>
        ///         <term> <see cref="TOKEN.Register"/> <see cref="TOKEN.Label"/> </term>
        ///         <description> Stores byte value from the specified register into the address of the given label. </description>
        ///     </item>
        /// </list>
        /// </summary>
        [Description("Store Instruction: Stores byte value from the specified register into the address indicated by the value of the given register.")]
        STB = 11,
        #endregion

        // Arithmetic Instructions
        ADD = 13,
        ADI = 14,
        SUB = 15,
        MUL = 16,
        DIV = 17,

        // Logical Instructions
        AND = 18,
        OR = 19,

        // Compare Instructions
        CMP = 20,

        // Traps
        /// <summary>
        /// Input/Output value depending on specified number.
        /// </summary>
        [Description("Input/Output value depending on specified number."), Category("Special Operations")]
        TRP = 21,

        // Multi-Threading Instructions
        [Description("Input/Output value depending on specified number."), Category("Special Operations")]
        RUN = 22,
        [Description("Input/Output value depending on specified number."), Category("Special Operations")]
        END = 23,
        [Description("Input/Output value depending on specified number."), Category("Special Operations")]
        BLK = 24,
        [Description("Input/Output value depending on specified number."), Category("Special Operations")]
        LCK = 25,
        [Description("Input/Output value depending on specified number."), Category("Special Operations")]
        ULK = 26

    }
}
