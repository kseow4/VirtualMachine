using System;
using VirtualMachine.Enumerations;
using VirtualMachine.Interfaces;
using VirtualMachine.Threading.States;

namespace VirtualMachine.Threading
{
    public class Thread
    {
        public THREAD ID { get; }
        public IState State { get; set; }
        public bool Locked { get; set; }
        public bool Enabled { get; set; }
        public bool Blocked { get; set; }
        public Thread(THREAD identifier, IState state = null, bool enabled = true) => (ID, State, Enabled) = (identifier, state ?? new Inactive(), enabled);

        public Thread(THREAD identifier, IState state) => (ID, State) = (identifier, state);

        public void Block() { if (ID is THREAD.Main) Blocked = true; }
        public void Unblock() { if (ID is THREAD.Main) Blocked = false; }
        public bool IsActive() => State is Active;
        public void Activate() => State = new Active();
        public void Deactivate() => State = new Inactive();
        public void Lock() => Locked = true;
        public void Unlock() => Locked = false;
        public bool Available() => State is Inactive;
        public override string ToString() => $"Thread({ID})";
    }
}
