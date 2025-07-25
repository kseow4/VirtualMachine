using System;
using System.Collections.Generic;
using System.Data;
using VirtualMachine.Enumerations;
using VirtualMachine.Interfaces;
using VirtualMachine.Threading.States;
using static System.Activator;

namespace VirtualMachine.VMTools
{
    public class ToolFactory
    {

        public IState NewState(object type) => (IState)CreateInstance(type.GetType() is IState ? type.GetType() : throw new InvalidCastException("Invalid object"));
        public IState NewState(Type type = null) => (IState)CreateInstance((type is IState ? type : throw new ConstraintException("Invalid type")) ?? typeof(Inactive));
        public IState ChangeState(Type type) => NewState(type);
        public bool Available(IState state) => state is Inactive;
        public bool Available(KeyValuePair<THREAD, IState> thread) => thread.Value is Inactive;
        public void Activate(ref IState state) => state = new Active();
        public void Activate(ref KeyValuePair<THREAD, IState> thread) => thread = new KeyValuePair<THREAD, IState>(thread.Key, new Active());
        public KeyValuePair<THREAD, IState> Activate(THREAD ID) => new KeyValuePair<THREAD, IState>(ID, new Active());

        public IState Deactivate() => ChangeState(typeof(Inactive));

    }
}
