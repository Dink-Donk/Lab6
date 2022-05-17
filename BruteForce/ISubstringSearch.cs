using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearch
{
    public interface ISubstringSearch
    {
        List<int> SubstringSearch(string text, string pattern);
    }
}
