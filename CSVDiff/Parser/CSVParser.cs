using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDiff.Parser
{
    class CSVParser : IRowParser
    {
        private const char COMMA = ',';
        private const string QUOTE = "\"";

        public List<string> ReadLine(string line)
        {
            string[] split = line.Split(COMMA);

            StringBuilder tmp = null;

            List<string> columns = new List<string>();

            bool inQuote = false;
            foreach (string s in split)
            {
                if (s.StartsWith(QUOTE))
                {
                    inQuote = true;
                    tmp = new StringBuilder();
                }
                else if (inQuote)
                    tmp.Append(COMMA);

                if (inQuote)
                {
                    tmp.Append(s);

                    if (s.EndsWith(QUOTE))
                    {
                        string txt = tmp.ToString();
                        txt = txt.Replace(QUOTE, "");
                        columns.Add(txt);

                        inQuote = false;
                    }
                }
                else
                    columns.Add(s);
            }

            return columns;
        }
    }
}
