using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWrapping
{
    public class SmawkBreaker : LineBreakerBase, ILineBreaker
    {
        const double LargeValue = double.PositiveInfinity;
        int width;
        int count;
        List<long> offsets;
        List<string> words;
        List<double> minima;
        List<long> breaks;

        private double CostFunction(long i, long j)
        {
            var w = offsets[(int)j] - offsets[(int)i] + j - i - 1; // width in characters of proposed split. TODO: sum up the character sizes?
            if (w > width) return LargeValue; // off the end
            w -= width; // mismatch
            return minima[(int)i] + ( w * w ); // add to sum-of-squares
        }

        private void smawk(long[] rows, long[] columns) {
            var stack = new Stack<long>();
            var i = 0;
            var j = 0;
            while (i < rows.Length)
            {
                if (stack.Count > 0) {
                    var c = columns[stack.Count - 1];
                    if (CostFunction(stack.Peek(), c) < CostFunction(rows[i], c)) {
                        if (stack.Count < columns.Length) stack.Push(rows[i]);
                        i++;
                    } else {
                        stack.Pop();
                    }
                } else {
                    stack.Push(rows[i]);
                    i++;
                }
            }
            rows = stack.ToArray();

            if (columns.Length > 1) smawk(rows, Evens(columns));

            i = 0;
            while (j < columns.Length){
                var end = j+1 < columns.Length ? breaks[(int)columns[j+1]] : rows[rows.Length - 1];

                var c = CostFunction(rows[i], columns[j]);
                if (c < minima[(int) columns[j]]) {
                    minima[(int) columns[j]] = c;
                    breaks[(int) columns[j]] = rows[i];
                }

                if (rows[i] < end) i ++;
                else j += 2;
            }
        }

        private long[] Evens(long[] columns)
        {
            var outp = new long[columns.Length / 2];
            for (int i = 0; i < columns.Length - 1; i+=2)
            {
                outp[i>>1] = columns[i+1];
            }
            return outp;
        }

        public string[] BreakLines(string text, int lineWidth)
        {
            width = lineWidth;
            words = ClassDivide(text);
            count = words.Count;

            offsets = new List<long>();
            breaks = new List<long>(Enumerable.Repeat(0L, count + 1));

            minima = new List<double>(Enumerable.Repeat(double.MaxValue, count + 1));
            minima[0] = 0.0d;

            offsets = new List<long>();
            var prev = 0L;
            foreach (var w in words)
            {
                offsets.Add(prev + w.Length);
                prev += w.Length;
            }
            offsets.Add(prev);

            var n = count + 1;
            var i = 0L;
            var offset = 0;
            while (true) {
                var r = (long)Math.Min(n, Math.Pow(2, i+1));
                var edge = (long)Math.Pow(2, i) + offset; // maybe Math.Pow(2, i + offset)  ??
                smawk(Range(offset, edge), Range(edge, r + offset));
                var x = minima[(int)(r - 1 + offset)];
                var exit = false;
                for (int j = (int)Math.Pow(2, i); j < (r - 1); j++)
                {
                    var y = CostFunction(j + offset, r - 1 + offset);
                    if (y <= x) {
                        n -= j;
                        i = 0;
                        offset += j;
                        exit = true;
                        break;
                    }
                }
                if (!exit){
                    if (r == n) break;
                    i++;
                }

            }
            
            var lines = new List<string>();
            long k = count;
            while (k > 0) {
                i = breaks[(int)k];
                lines.Add(RangeJoin(words, i, k));
                k = i;
            }
            lines.Reverse();
            return lines.ToArray();
        }

        private long[] Range(long from, long upto)
        {
            return Enumerable.Range((int)from, (int)(upto - from)).Select(i => (long)i).ToArray();
        }
    }
}