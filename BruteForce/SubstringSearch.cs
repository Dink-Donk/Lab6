using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearch
{
    public class KMP : ISubstringSearch
    {
        public KMP() { }
        private int[] FindBorders(string pattern)
        {
            int[] borders = new int[pattern.Length];
            borders[0] = 0;
            for (int i = 1; i < pattern.Length; i++)
            {
                int index = borders[i - 1];
                while (index > 0 && pattern[index] != pattern[i])
                {
                    index = borders[index - 1];
                }
                if (pattern[index] == pattern[i])
                {
                    index++;
                }
                borders[i] = index;
            }
            return borders;
        }
        public List<int> SubstringSearch(string text, string pattern)
        {
            var borders = FindBorders(pattern);
            List<int> result = new List<int>();
            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
                while (index > 0 && text[i] != pattern[index])
                {
                    index = borders[index - 1];
                }
                if (text[i] == pattern[index])
                {
                    index++;
                }
                if (index == pattern.Length)
                {
                    result.Add(i - index + 1);
                    index = borders[pattern.Length - 1];
                }
            }
            return result;
        }
    }

    public class RK : ISubstringSearch
    {
        private const byte mod = 10;
        private const byte d = 8;
        public RK() { }
        public List<int> SubstringSearch(string text, string pattern)
        {
            List<int> result = new List<int>();
            var patternH = 0;
            var textH = 0;
            var firstIndexHash = 1;
            for (int i = 0; i < pattern.Length; i++)
            {
                patternH = ((patternH << d) + pattern[i]);
                patternH -= (patternH >> mod << mod);
                textH = ((textH << d) + text[i]);
                textH -= (textH >> mod << mod);
            }
            for (int i = 1; i < pattern.Length; i++)
            {
                firstIndexHash = (firstIndexHash << d);
                firstIndexHash -= (firstIndexHash >> mod << mod);
            }
            for (int i = 0; i < text.Length - pattern.Length + 1; i++)
            {
                if (textH == patternH)
                {
                    bool flag = true;
                    for (int j = 0; j < pattern.Length; j++)
                    {
                        if (text[i + j] != pattern[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        result.Add(i);
                    }
                }
                else if (i != text.Length - pattern.Length)
                {
                    textH = (((textH - text[i] * firstIndexHash) << d) + text[i + pattern.Length]);
                    textH -= (textH >> mod << mod);
                }
            }
            return result;
        }
    }
    public class BruteForce : ISubstringSearch
    {
        public List<int> SubstringSearch(string text, string pattern)
        {
            int m = pattern.Length;
            int n = text.Length;
            List<int> shiftCollection = new List<int>();
            if (n == 0 || m == 0)
            {
                return shiftCollection;
            }
            for (int shift = 0; shift <= n - m; shift++)
            {
                bool SubStringIsEqual = true;
                for (int i = 0; i < m && SubStringIsEqual; i++)
                {
                    if (pattern[i] != text[shift + i])
                    {
                        SubStringIsEqual = false;
                    }
                }
                if (SubStringIsEqual)
                {
                    shiftCollection.Add(shift);
                }
            }

            return shiftCollection;
        }
    }

    public class BoyerMoore : ISubstringSearch
    {
        private int[] GetBadCharShifts(string s)
        {
            var array = new int[10000];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = -1;
            }
            for (int i = 0; i < s.Length; i++)
            {
                array[(int)s[i]] = i;
            }
            return array;
        }

        private int[] GetReverseZFunction(string s)
        {
            var z = new int[s.Length];
            var left = 0;
            var right = 0;
            for (var i = 1; i < s.Length; i++)
            {
                if (i <= right)
                    z[i] = Math.Min(z[i - left], right - i + 1);
                while (z[i] + i < s.Length && s[s.Length - 1 - (i + z[i])] == s[s.Length - 1 - z[i]])
                    z[i]++;
                if (i + z[i] - 1 > right)
                {
                    right = i + z[i] + 1;
                    left = i;
                }
            }
            return z;
        }
        private int[] GetGoodSuffixShifts(string s)
        {
            var result = new int[s.Length + 1];
            var reverseZFunction = GetReverseZFunction(s);

            for (var i = 0; i < result.Length; i++)
                result[i] = s.Length;

            for (var i = s.Length - 1; i > 0; i--)
                result[s.Length - reverseZFunction[i]] = i;

            var currentPref = 0;
            for (var j = 1; j < s.Length; j++)
                if (j + reverseZFunction[j] == s.Length)
                    for (; currentPref <= j; currentPref++)
                        if (result[currentPref] == s.Length)
                            result[currentPref] = j;

            return result;
        }
        public List<int> SubstringSearch(string text, string pattern)
        {
            List<int> result = new List<int>();
            int m = pattern.Length;
            int n = text.Length;
            if (n == 0 || m == 0)
            {
                return result;
            }
            var badCharShifts = GetBadCharShifts(pattern);
            var goodSuffixShifts = GetGoodSuffixShifts(pattern);
            int bound = 0;
            for (int i = 0; i <= n - m;)
            {
                int j;
                for (j = m - 1; j >= bound && pattern[j] == text[i + j]; j--) ;

                if (j < 0)
                {
                    result.Add(i);
                    i += goodSuffixShifts[j + 1];
                }
                else
                {
                    i += Math.Max(goodSuffixShifts[j + 1], j - badCharShifts[text[i + j]]);
                }
            }
            return result;
        }
    }
}
