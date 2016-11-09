using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSVDiff.Formatter
{
    class RemoveWhiteSpace : IFormatter
    {
        public string Format(string data)
        {
            data = data.Replace(" ", "");
            return data;
        }
    }
}
