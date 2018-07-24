using System;

namespace LineWrapping
{
    internal class LongByLong : Tuple<long, long>
    {
        public LongByLong(long item1, long item2) : base(item1, item2)
        {
        }

        public static implicit operator LongByLong((long, long) valueTuple) {
            return new LongByLong(valueTuple.Item1, valueTuple.Item2);
        }

        public long this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;

                    default: throw new Exception("Not implemented");
                }
            }
        }
    }
}