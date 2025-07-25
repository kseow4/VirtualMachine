using System;
using VirtualMachine.Enumerations;

namespace VirtualMachine.Interfaces
{
    public interface IThread
    {
        Enum ID { get; }
       // T IDs { get; }
        IState State { get; set; }
        bool Locked { get; set; }
        bool Blocked { get; set; }

        //Thread(THREAD identifier, IState state = null) => (ID, State) = (identifier, state ?? new Inactive());
        void Block();
        void Unblock();
        void Activate();
        void Deactivate();
        void Lock();
        void Unlock();
        bool Available();
    }
}
