using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationLib
{
    public class FixedSizedStack<T> : Stack<T>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizedStack(int size)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Push(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    T outObj = base.Pop();
                }
            }
        }

        public T Dequeue()
        {
            T outObj = base.Pop();
            return outObj;
        }
    }
}
