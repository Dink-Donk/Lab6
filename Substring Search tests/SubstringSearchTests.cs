using System;
using System.Collections.Generic;
using Xunit;
using SubstringSearch;
using System.Linq;

namespace Substring_Search_tests
{
    public class SubstringSearchTest
    {

        [Theory]
        [InlineData("a", "aaa", new int[] { 0, 1, 2 })]
        [InlineData("aa", "aaaaaa", new int[] { 0, 1, 2, 3, 4 })]
        [InlineData("abc", "abcabcabc", new int[] { 0, 3, 6 })]
        [InlineData("abra", "abracadabraabracadabra", new int[] { 0, 7, 11, 18 })]
        [InlineData("a", "ababab", new int[] { 0, 2, 4 })]
        [InlineData("a", "abbbbb", new int[] { 0 })]
        [InlineData("a", "bbbbba", new int[] { 5 })]
        [InlineData("a", "bababa", new int[] { 1, 3, 5 })]
        [InlineData("abcabcabc", "abcabcabc", new int[] { 0 })]
        public void SearchTest(string pattern, string text, IEnumerable<int> expected)
        {
            var algms = new List<ISubstringSearch>()
            {
                new BoyerMoore(),
                new BruteForce(),
                new KMP(),
                new RK()
            };
            foreach (var algm in algms)
            {
                var actual = algm.SubstringSearch(text, pattern);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void FindAllShiftsInTextThatConsistsOnlyOfThisPattern()
        {
            var algms = new List<ISubstringSearch>()
  {
      new BoyerMoore(),
      new BruteForce(),
      new KMP(),
      new RK()
   };
            string text = "aaaaaaaaaa"; //10
            string pattern = "aa";
            var expected = Enumerable.Range(0, 9).ToList();
            foreach (var algm in algms)
            {
                var actual = algm.SubstringSearch(text, pattern);
                Assert.Equal(expected.Count, actual.Count);
                for (int i = 0; i < actual.Count; i++)
                    Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void FindAllShiftsInTextThatHasMupltiplePartsOfThePatternInTheRow()
        {
            var algms = new List<ISubstringSearch>()
  {
      new BoyerMoore(),
      new BruteForce(),
      new RK(),
      new KMP()
   };
            string text = "ASDXCASCABCABCABASDCXAWCABDD";
            string pattern = "CABCAB";
            int[] expected = new int[] { 7, 10 };
            foreach (var algm in algms)
            {
                var actual = algm.SubstringSearch(text, pattern);
                Assert.Equal(expected.Length, actual.Count);
                for (int i = 0; i < expected.Length; i++)
                    Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void DoesNotFindNonExistingOccurancesOfThePattern()
        {
            var algms = new List<ISubstringSearch>()
  {
      new BoyerMoore(),
      new BruteForce(),
      new RK(),
      new KMP()
   };
            string text = "ASDXCASCABCABCABASDCXAWCABDD";
            string pattern = "PPPLLOI";
            int expected = 0;
            foreach (var algm in algms)
            {
                var actual = algm.SubstringSearch(text, pattern);
                Assert.Equal(expected, actual.Count);
            }
        }
    }
}
