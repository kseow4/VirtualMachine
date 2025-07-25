using System.ComponentModel;

namespace VirtualMachine.Enumerations
{
    /// <summary>
    /// Memory Registers
    /// </summary>
    [Description("Register ID")]
    public enum REGISTER
    {
        /// <summary> Register Zero </summary>
        [Description("Register Zero")]
        R0,
        /// <summary> Register One </summary>
        [Description("Register One")]
        R1,
        /// <summary> Register Two </summary>
        [Description("Register Two")]
        R2,
        /// <summary> Register Three - I/O Trap Routine Register </summary>
        [Description("Register Three - I/O Trap Routine Register")]
        R3,
        /// <summary> Register Four </summary>
        [Description("Register Four")]
        R4,
        /// <summary> Register Five </summary>
        [Description("Register Five")]
        R5,
        /// <summary> Register Six </summary>
        [Description("Register Six")]
        R6,
        /// <summary> Register Seven </summary>
        [Description("Register Seven")]
        R7,
        /// <summary> Register Eight - Program Counter (PC) </summary>
        [Description("Register Eight - Program Counter (PC)")]
        PC,
        /// <summary> Register Nine - Stack Limit </summary>
        [Description("Register Nine - Stack Limit")]
        SL,
        /// <summary> Register Ten - Stack Pointer </summary>
        [Description("Register Ten - Stack Pointer")]
        SP,
        /// <summary> Register Eleven - Frame Pointer </summary>
        [Description("Register Eleven - Frame Pointer")]
        FP,
        /// <summary> Register Twelve - Stack Base </summary>
        [Description("Register Twelve - Stack Base")]
        SB
    }
}
