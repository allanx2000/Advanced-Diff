using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDiff.Parser
{
    interface IRowParser
    {
        List<string> ReadLine(string line);
    }
}
