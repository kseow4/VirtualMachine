using System.Runtime.CompilerServices;
using System.Reflection;
using VirtualMachine.Interfaces;
using System;

namespace VirtualMachine.Threading.States
{
    public abstract class State : IState
    {
        public virtual bool Blocked { get; set; } = false;
        public virtual void Block() => Blocked = true;
        public virtual void Unblock() => Blocked = false;
    }
}
