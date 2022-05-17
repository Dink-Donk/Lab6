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
            string pattern = "Лев Толстой";
            BoyerMoore bm = new BoyerMoore();
            KMP kmp = new KMP();
            RK RK = new RK();
            BruteForce brute = new BruteForce();
            List<int> BMOutput = new List<int>();
            List<int> KMPOutput = new List<int>();
            List<int> RKOutput = new List<int>();
            List<int> BruteOutput = new List<int>();

            var sw = new Stopwatch();

            sw.Restart();
            BMOutput = bm.SubstringSearch(input, pattern);
            sw.Stop();
            Console.WriteLine("Boyer Moore: " + sw.ElapsedTicks);
            Console.WriteLine("Всего: " + BMOutput.Count);
            Console.WriteLine("---------------------------------------");

            sw.Restart();
            KMPOutput = kmp.SubstringSearch(input, pattern);
            sw.Stop();
            Console.WriteLine("KMP: " + sw.ElapsedTicks);
            Console.WriteLine("Всего: " + KMPOutput.Count);
            Console.WriteLine("---------------------------------------");

            sw.Restart();
            RKOutput = RK.SubstringSearch(input, pattern);
            sw.Stop();
            Console.WriteLine("RK: " + sw.ElapsedTicks);
            Console.WriteLine("Всего: " + RKOutput.Count);
            Console.WriteLine("---------------------------------------");

            sw.Restart();
            BruteOutput = brute.SubstringSearch(input, pattern);
            sw.Stop();
            Console.WriteLine("Brute Force: " + sw.ElapsedTicks);
            Console.WriteLine("Всего: " + BruteOutput.Count);
            Console.WriteLine("---------------------------------------");
            Console.ReadKey();
        }
    }
}
