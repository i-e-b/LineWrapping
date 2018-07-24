using System;
using System.Diagnostics;
using System.IO;

namespace LineWrapping
{
    class Program
    {
        // http://xxyxyz.org/line-breaking/
        static void Main(string[] args)
        {
            // 1) Read in a text file,
            // 2) Remove line breaks
            // 3) restore line breaks with line-breaking algo
            // 4) write output

            var columnWidth = 40;
            var data = File.ReadAllText(@"C:\Temp\CMNetlog.txt");

            // Shorter data for debug
            //data = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            //data = "one            \r\n                            two three four five six-seven-eight 9-10-11:12 thirteen fourteen fifteen sixteen seventeen eighteen nineteen twenty";

            Console.WriteLine(new string('@', columnWidth));

            var sw = new Stopwatch();
            sw.Start();

            var br = new BinarySearchBreaker();
            //var br = new SmawkBreaker();
            var outp = br.BreakLines(data, columnWidth);

            sw.Stop();

            var blob = string.Join("\r\n", outp);
            Console.WriteLine(blob);

            Console.WriteLine("\r\nDone. Took "+sw.Elapsed+". Press [Enter]");
            Console.ReadKey();
        }
    }
}
