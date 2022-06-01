using SubstringSearch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubString
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input;
            using (var sr = new StreamReader("anna.txt", Encoding.Default))
            {
                input = sr.ReadToEnd();
            }
            string pattern = "по";
            BoyerMoore bm = new BoyerMoore();
            KMP kmp = new KMP();
            RK RK = new RK();
            BruteForce brute = new BruteForce();
            List<int> BMOutput = new List<int>();
            List<int> KMPOutput = new List<int>();
            List<int> RKOutput = new List<int>();
            List<int> BruteOutput = new List<int>();

            var sw = new Stopwatch();
            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
            var sw3 = new Stopwatch();

            sw.Start();
            BMOutput = bm.SubstringSearch(input, pattern);
            sw.Stop();
            Console.WriteLine("Boyer Moore: " + (sw.ElapsedTicks));
            Console.WriteLine("Всего: " + BMOutput.Count);
            Console.WriteLine("---------------------------------------");

            sw1.Start();
            KMPOutput = kmp.SubstringSearch(input, pattern);
            sw1.Stop();
            Console.WriteLine("KMP: " + sw1.ElapsedTicks) ;
            Console.WriteLine("Всего: " + KMPOutput.Count);
            Console.WriteLine("---------------------------------------");

            sw2.Start();
            RKOutput = RK.SubstringSearch(input, pattern);
            sw2.Stop();
            Console.WriteLine("RK: " + sw2.ElapsedTicks);
            Console.WriteLine("Всего: " + RKOutput.Count);
            Console.WriteLine("---------------------------------------");

            sw3.Start();
            BruteOutput = brute.SubstringSearch(input, pattern);
            sw3.Stop();
            Console.WriteLine("Brute Force: " + sw3.ElapsedTicks);
            Console.WriteLine("Всего: " + BruteOutput.Count);
            Console.WriteLine("---------------------------------------");
            Console.ReadKey();
        }
    }
}
