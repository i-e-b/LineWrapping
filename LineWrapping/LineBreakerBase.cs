using System.Collections.Generic;
using System.Text;

namespace LineWrapping
{
    public class LineBreakerBase
    {
        public List<string> ClassDivide(string text)
        {
            var outp = new List<string>();

            var sb = new StringBuilder();
            int wsl = 0;
            foreach (char c in text)
            {
                //var cat = char.GetUnicodeCategory(c);
                //switch (cat) { ...

                if (char.IsWhiteSpace(c)) {
                    if (wsl < 1) {
                        if (c == '\r' || c != '\n') sb.Append(' ');
                        else sb.Append(c);
                    }
                    if (sb.Length > 0) outp.Add(sb.ToString());
                    sb.Clear();
                    wsl++;
                    continue;
                }

                wsl = 0;

                if (char.IsPunctuation(c)) {
                    sb.Append(c);
                    outp.Add(sb.ToString());
                    sb.Clear();
                    continue;
                }

                if (char.IsLetter(c) || char.IsDigit(c))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length > 0) outp.Add(sb.ToString());
                sb.Clear();
                outp.Add(c.ToString());

            }

            if (sb.Length > 0) outp.Add(sb.ToString());
            return outp;
        }

        public string RangeJoin(List<string> bits, long idx1, long idx2)
        {
            var sb = new StringBuilder();
            for (int i = (int)idx1; i < (int)idx2; i++)
            {
                sb.Append(bits[i]);
            }
            return sb.ToString();
        }
    }
}