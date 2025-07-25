using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VirtualMachine.Enumerations;
using VirtualMachine.Threading;
using System.Data;
using VirtualMachine.Interfaces;
using static VirtualMachine.VMTools.VM_Extensions;
using VirtualMachine.Threading.States;

namespace VirtualMachine.Threading
{
    public class ThreadQueue : List<Thread>
    {
        public ThreadQueue() : base() { }
    //    public ThreadQueue(int threads) : base(ZipEnums<THREAD, Inactive, Thread>(threads)) { }
        public ThreadQueue(int threads) : base(ZipEnums<THREAD, Inactive, Thread>(threads)) { }

        public ThreadQueue(IList<Thread> ts) : base(ts) { }
        public ThreadQueue(List<Thread> ts) : base(ts) { }
        public ThreadQueue(ICollection<Thread> collection) : base(collection) { }
        public ThreadQueue(IEnumerable<Thread> collection) : base(collection) { }
        public Thread this[Thread id] { get => this[(int)id.ID]; set => this[(int)id.ID] = value; }
        public Thread this[THREAD id] { get => this[(int)id]; set => this[(int)id] = value; }
      //  public new void Add(Thread thread) { if (!this.Any(t => t.ID is THREAD.Main)) { base.Add(thread); } throw new DuplicateNameException(thread.ToString()); }

        

   //     public ThreadQueue(ICollection<T> collection) => Threads = new List<T>(collection);
   //     public ThreadQueue(IEnumerable<T> collection) => Threads = new List<T>(collection);
   //     public ThreadQueue(IList<T> collection) => Threads = new List<T>(collection);
  //      public ThreadQueue(List<T> collection) => Threads = new List<T>(collection);

    //    public ThreadQueue(params Thread[] threads) => Threads = new List<Thread>(threads);


     //   public List<Thread> Threads = Enum.GetValues(typeof(THREAD)).Cast<THREAD>().ToList().Zip(Enumerable.Repeat(new Inactive(), Enum.GetValues(typeof(THREAD)).Length), (id, thread) => new Thread(id)).ToList();

     //   public Thread this[THREAD id] { get => Threads[(int)id]; set => Threads[(int)id] = value; }

     //   public void Add(Thread thread) => Threads.Add(thread);


    }
}
