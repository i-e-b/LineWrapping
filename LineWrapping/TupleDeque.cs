using System;
using System.Collections.Generic;

namespace LineWrapping
{
    internal class TupleDeque
    {
        LinkedList<LongByLong> q = new LinkedList<LongByLong>();

        public TupleDeque((int, int) valueTuple)
        {
            q.AddFirst(new LongByLong(valueTuple.Item1, valueTuple.Item2));
        }

        public LongByLong this[int slice]
        {
            get {
                switch (slice) {
                    case -1:
                        return q.Last.Value;
                    case 0:
                        return q.First.Value;
                    case 1:
                        return q.First.Next?.Value;

                    default: throw new Exception("Not implemented");
                }
            }
            set {
                switch (slice) {
                    case -1:
                        q.Last.Value = value;
                        return;
                    case 0:
                        q.First.Value = value;
                        return;
                    case 1:
                        if (q.First.Next == null) throw new Exception("out of range");
                        q.First.Next.Value = value;
                        return;

                    default: throw new Exception("Not implemented");
                }
            }
        }

        public void ClearTo((int, int) valueTuple)
        {
            q.Clear();
            q.AddFirst(new LongByLong(valueTuple.Item1, valueTuple.Item2));
        }

        public void PopRight()
        {
            q.RemoveLast();
        }

        public void PushRight((long, long) valueTuple)
        {
            q.AddLast(new LongByLong(valueTuple.Item1, valueTuple.Item2));
        }

        public void PopLeft()
        {
            q.RemoveFirst();
        }
    }
}