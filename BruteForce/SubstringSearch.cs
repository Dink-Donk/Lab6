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
            for (int i = 1; i < patternSB.Length; i++)
            {
                int index = borders[i - 1];
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
        private const int mod = 17;
        private const int d = 128;
        public RK() { }
        public List<int> SubstringSearch(string text, string pattern)
        {
            List<int> result = new List<int>();
            var patternH = 0;
            var textH = 0;
            var firstIndexHash = (int)Math.Pow(d, pattern.Length - 1) % mod;
            for (int i = 0; i < pattern.Length; i++)
            {
                patternH = (d * patternH + pattern[i]) % mod;
                textH = (d * textH + text[i]) % mod;
            }
            if (patternH < 0) { patternH += mod; }
            if (textH < 0) { textH += mod; }
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
                else if (i < text.Length - pattern.Length)
                {
                    textH = (d * (textH - text[i] * firstIndexHash) + text[i + pattern.Length]) % mod;
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
            //Dictionary<char, int> badCharTable = new Dictionary<char, int>();
            //for (int i = pattern.Length - 2; i >= 0; i--)
            //{
            //    if (!badCharTable.ContainsKey(pattern[i]))
            //    {
            //        badCharTable.Add(pattern[i], pattern.Length - i - 1);
            //    }
            //}

            //Bad char
            int[] badCharTable = new int[10000];
            for (int i = pattern.Length - 2; i >= 0; i--)
            {
                int symbol = pattern[i];
                if (badCharTable[symbol]==0)
                {
                    badCharTable[symbol]=pattern.Length - i-1;
                }
            }

            //Good suff
            int[] goodSuffShift = new int[pattern.Length];

            //Case 1, suffix exist somewhere in the pattern
            int suffixLength = 1;
            for (int i = pattern.Length - 1; i >= 0; i--)
            {

                for (int j = i - 1; j >= 0; j--)
                {
                    int currSubstringToCheckIndexer = j;
                    int currSuffixToFindIndexer = i;
                    bool foundMatch = true;
                    for (int howFarWeGo = suffixLength; howFarWeGo > 0; howFarWeGo--, currSubstringToCheckIndexer++, currSuffixToFindIndexer++)
                    {
                        if (pattern[currSubstringToCheckIndexer] != pattern[currSuffixToFindIndexer])
                        {
                            foundMatch = false;
                            break;
                        }
                    }
                    if (foundMatch)
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
                            if (badCharTable[text[textIndex]] != 0)
                            {
                                badCharSkip = badCharTable[text[textIndex]];
                            }
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
