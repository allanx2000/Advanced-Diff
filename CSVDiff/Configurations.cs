using System;
using System.Collections.Generic;

namespace CSVDiff
{
    internal class Configurations
    {
        public int KeyColumn { get; internal set; }

        private Dictionary<int, string> formatters = new Dictionary<int, string>();

        public Configurations(int keyColumn = 0)
        {
            KeyColumn = keyColumn;
        }

        public void SetFormatter(int col, string formatter)
        {
            formatters[col] = formatter;
        }

        public string GetFormatter(int col)
        {
            if (formatters.ContainsKey(col))
                return formatters[col];
            else
                return null;
        }
    }
}