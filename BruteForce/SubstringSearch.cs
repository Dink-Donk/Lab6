using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearch
{
    public class KMP : ISubstringSearch
    {
        public KMP() { }
        private int[] FindBorders(string pattern)
        {
            StringBuilder patternSB = new StringBuilder(pattern);
            int[] borders = new int[patternSB.Length];
            borders[0] = 0;
            int index = 0;
            for (int i = 1; i < patternSB.Length; i++)
            {
                while (index > 0 && patternSB[index] != patternSB[i])
                {
                    index = borders[index - 1];
                }
                if (patternSB[index] == patternSB[i])
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
            StringBuilder textSB = new StringBuilder(text);
            StringBuilder patternSB = new StringBuilder(pattern);
            for (int i = 0; i < text.Length; i++)
            {
                while (index > 0 && textSB[i] != patternSB[index])
                {
                    index = borders[index - 1];
                }
                if (text[i] == patternSB[index])
                {
                    index++;
                }
                if (index == patternSB.Length)
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
        private const int mod = 2001;
        private const int d = 128;
        public RK() { }
        public List<int> SubstringSearch(string text, string pattern)
        {
            StringBuilder textSB = new StringBuilder(text);
            StringBuilder patternSB = new StringBuilder(pattern);
            List<int> result = new List<int>();
            var patternH = patternSB[0] % mod;
            var textH = textSB[0] % mod;
            var firstIndexHash = 1;
            for (int i = 1; i < pattern.Length; i++) 
            {
                patternH = (patternH * d + patternSB[i]) % mod;                
                textH = (textH * d + textSB[i]) % mod;
                firstIndexHash = (firstIndexHash * d) % mod;
                if (firstIndexHash < 0) { firstIndexHash += mod; }
            }
            if (patternH < 0) { patternH += mod; }
            if (textH < 0) { textH += mod; }
            for (int i = 0; i < text.Length - pattern.Length + 1; i++)
            {
                if (textH == patternH)
                {
                    if(new StringBuilder(text.Substring(i, pattern.Length)).Equals(patternSB))
                    {
                        result.Add(i);
                    }
                }
                if (i < text.Length - pattern.Length) 
                {
                    textH = (d * (textH - textSB[i] * firstIndexHash) + textSB[i + patternSB.Length]) % mod;
                    if (textH < 0) { textH += mod; }
                }
            }
            return result;
        }
    }

    public class BruteForce : ISubstringSearch
    {
        public List<int> SubstringSearch(string text, string pattern)
        {
            List<int> patternIndexes = new List<int>();
            for (int tIterator = 0; tIterator < text.Length; tIterator++)
            {
                int pIterator;
                if (tIterator + pattern.Length > text.Length)
                    break;
                for (pIterator = 0; pIterator < pattern.Length; pIterator++)
                {
                    if (text[tIterator + pIterator] != pattern[pIterator])
                        break;
                }
                if (pIterator == pattern.Length) patternIndexes.Add(tIterator);
            }
            return patternIndexes;
        }
    }

    public class BoyerMoore : ISubstringSearch
    {
        public List<int> SubstringSearch(string text, string pattern)
        {
            List<int> patternIndexes = new List<int>();

            //Bad char 
            Dictionary<char, int> badCharTable = new Dictionary<char, int>();
            for (int i = pattern.Length - 2; i >= 0; i--)
            {
                if (!badCharTable.ContainsKey(pattern[i]))
                {
                    badCharTable.Add(pattern[i], pattern.Length - i - 1);
                }
            }

            //Good suff
            int[] goodSuffShift = new int[pattern.Length];

            //Case 1, suffix exist somewhere in the pattern
            int suffixLength = 1;
            for (int i = pattern.Length - 1; i >= 0; i--)
            {
                string currSuffixToFind = pattern.Substring(i);
                for (int j = i - 1; j >= 0; j--)
                {
                    string currSubstringToCheck = pattern.Substring(j, suffixLength);
                    if (currSuffixToFind == currSubstringToCheck)
                        goodSuffShift[i] = i - j;
                }
                suffixLength++;
            }

            //Case 2, part of the suffix exists as prefix
            suffixLength = 1;
            for (int i = pattern.Length - 1; i >= 0; i--)
            {
                string currSuffixToFind = pattern.Substring(i);
                int suffDecr = 0;
                if (goodSuffShift[i] != 0)
                {
                    suffixLength++;
                    continue;
                }
                for (int j = suffixLength; j > 0; j--)
                {
                    string currPrefixToCheck = pattern.Substring(0, j);
                    if (currSuffixToFind == currPrefixToCheck)
                    {
                        goodSuffShift[i] = pattern.Length - j;
                        break;
                    }
                    else
                    {
                        suffDecr++;
                        currSuffixToFind = pattern.Substring(i + suffDecr);
                    }
                }
                suffixLength++;
            }

            //Search itself  
            for (int i = 0; i < text.Length; i++)
            {
                if (i + pattern.Length - 1 < text.Length)
                {
                    int patternIndex = pattern.Length - 1;
                    for (int textIndex = i + pattern.Length - 1; textIndex >= i; textIndex--, patternIndex--)
                    {
                        //Skip
                        if (text[textIndex] != pattern[patternIndex])
                        {
                            int badCharSkip = 1;
                            int goodSuffSkip = 1;
                            if (badCharTable.ContainsKey(text[textIndex]))
                                badCharSkip = badCharTable[text[textIndex]];
                            else
                                badCharSkip = pattern.Length;
                            if (pattern.Length - patternIndex - 2 > 0)
                            {
                                goodSuffSkip = goodSuffShift[pattern.Length - patternIndex - 2];
                            }
                            var skip = Math.Max(badCharSkip, goodSuffSkip);
                            i += skip;
                            i--;
                            break;
                        }
                    }
                    if (patternIndex == -1)
                        patternIndexes.Add(i);
                }
            }
            return patternIndexes;
        }
    }
}
