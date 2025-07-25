namespace VirtualMachine.Interfaces
{
    public interface IState
    {
        bool Blocked { get; set; }
        void Block();
        void Unblock();

    }
}
