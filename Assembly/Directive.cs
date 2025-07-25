using System;
using VirtualMachine.Enumerations;

namespace VirtualMachine.Assembly
{
    public abstract class Directive
    {
        public virtual DIRECTIVE DirectiveType { get; protected set; }
        public virtual string Label { get; protected set; }
        public virtual object Value { get; protected set; }

        public static Directive GetDirective(DIRECTIVE directiveType, int value, string label = null)
        {
            switch (directiveType)
            {
                case DIRECTIVE.BYT: return new BytDirective((byte)value, label);
                case DIRECTIVE.INT: return new IntDirective(value, label);
                case DIRECTIVE.ALN: throw new NotImplementedException();
                default: throw new NotImplementedException();
            }
        }

        internal class IntDirective : Directive
        {
            public new int Value { get; set; }
            public IntDirective(int value, string label = null) => (base.DirectiveType, Value, Label) = (DIRECTIVE.INT, value, label);

        }

        internal class BytDirective : Directive
        {
            public new byte Value { get; set; }
            public BytDirective(byte value, string label = null) => (base.DirectiveType, Value, Label) = (DIRECTIVE.BYT, value, label);
        }
    }
}
