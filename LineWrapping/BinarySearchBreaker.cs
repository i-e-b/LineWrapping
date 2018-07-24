using System.Collections.Generic;
using System.Linq;

namespace LineWrapping
{
    public class BinarySearchBreaker : LineBreakerBase, ILineBreaker
    {
        const double LargeValue = double.PositiveInfinity;
        List<long> offsets;
        int width;
        int count;
        List<string> words;
        List<double> minima;
        List<long> breaks;
        TupleDeque ranges;


        private double CostFunction(long i, long j)
        {
            var w = offsets[(int)j] - offsets[(int)i] + j - i - 1; // width in characters of proposed split. TODO: sum up the character sizes?
            if (w > width) return LargeValue; // off the end
            w -= width; // mismatch
            return minima[(int)i] + ( w * w ); // add to sum-of-squares
        }

        private long Search(long l, long k)
        {
            var low = l + 1;
            long high = count;

            while (low < high) {
                var mid = (low + high) / 2;
                if (CostFunction(l, mid) <= CostFunction(k,mid)) high = mid;
                else if (low == mid || mid == high) break;
                else low = mid;
            }
            if (CostFunction(l,high) <= CostFunction(k, high)) return high;
            return l + 2;
        }

        public string[] BreakLines(string text, int lineWidth)
        {
            width = lineWidth;
            words = ClassDivide(text);
            count = words.Count;

            offsets = new List<long>();
            minima = new List<double>(Enumerable.Repeat(0.0d, count + 1));
            breaks = new List<long>(Enumerable.Repeat(0L, count + 1));

            offsets = new List<long>();
            var prev = 0L;
            foreach (var w in words)
            {
                offsets.Add(prev + w.Length);
                prev += w.Length;
            }
            offsets.Add(prev);

            ranges = new TupleDeque((0,1));
            int j;
            for (j = 1; j < count + 1; j++)
            {
                var l = ranges[0][0];
                var leftCost = CostFunction(j-1, j);
                var rightCost = CostFunction(l,j);

                if (leftCost <= rightCost) {
                    minima[j] = leftCost;
                    breaks[j] = j - 1;
                    ranges.ClearTo((j-1, j+1));
                }
                else
                {
                    minima[j] = rightCost;
                    breaks[j] = l;

                    while (CostFunction(j-1, ranges[-1][1]) <= CostFunction(ranges[-1][0], ranges[-1][1])) ranges.PopRight();

                    ranges.PushRight((j-1, Search(j-1, ranges[-1][0])));
                    if (j+1 == ranges[1][1]) ranges.PopLeft();
                    else ranges[0] = (ranges[0][0],  ranges[0][1] + 1);
                }
            }

            var lines = new List<string>();
            long k = count;
            while (k > 0) {
                var i = breaks[(int)k];
                lines.Add(RangeJoin(words, i, k));
                k = i;
            }
            lines.Reverse();
            return lines.ToArray();
        }
    }
}