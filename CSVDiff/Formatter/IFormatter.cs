using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDiff.Formatter
{
    interface IFormatter
    {
        string Format(string data);
    }
}
