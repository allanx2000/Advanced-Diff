using CSVDiff.Formatter;
using CSVDiff.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDiff
{
    class Program
    {
        private const string NO_SPACE = "NO_SPACE";

        private static Configurations Configs;
        private static Dictionary<string, IFormatter> Formatters;
        private static IRowParser Parser;
        private static StreamWriter writer;
        
        static void Write(string text)
        {
            Console.WriteLine(text);
            writer.WriteLine(text);
        }

        static void Main(string[] args)
        {
            string leftFile, rightFile;
            string configFile = null;

            //leftFile = args[0];
            //rightFile = args[1];

            leftFile = "input1.csv";
            rightFile = "input2.csv";

            writer = new StreamWriter("output.txt");

            if (args.Length > 2)
                configFile = args[2];

            Configs = LoadConfiguration(configFile);
            Formatters = LoadFormatters();
            Parser = new CSVParser();

            Configs.SetFormatter(1, NO_SPACE);

            StreamReader srLeft = new StreamReader(leftFile);
            StreamReader srRight = new StreamReader(rightFile);

            Dictionary<string, List<string>> rowsLeft = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> rowsRight = new Dictionary<string, List<string>>();

            while (!srLeft.EndOfStream && !srRight.EndOfStream)
            {
                string strLeft = srLeft.ReadLine();
                string strRight = srRight.ReadLine();

                List<string> listLeft = Parser.ReadLine(strLeft);
                List<string> listRight = Parser.ReadLine(strRight);

                Clean(listLeft);
                Clean(listRight);

                string keyLeft = GetKey(listLeft);
                string keyRight = GetKey(listLeft);

                List<Diff> diffs;

                if (keyLeft == keyRight)
                {
                    diffs = Compare(listLeft, listRight, keyLeft);
                }
                else
                {
                    List<string> tmpList = GetRow(rowsLeft, keyRight);
                    if (tmpList != null)
                        diffs = Compare(tmpList, listRight, keyRight);
                    else
                        rowsRight.Add(keyRight, listRight);

                    tmpList = GetRow(rowsRight, keyLeft);
                    if (tmpList != null)
                        diffs = Compare(tmpList, listLeft, keyLeft);
                    else
                        rowsRight.Add(keyLeft, listLeft);
                }
            }

            ReadRemaining("Remaining Left------", srLeft);
            ReadRemaining("Remaining Right-----", srRight);

            srLeft.Close();
            srRight.Close();

            writer.Close();
        }

        private static void ReadRemaining(string title, StreamReader reader)
        {
            if (reader.EndOfStream)
                return;

            Write(title);
            while (!reader.EndOfStream)
            {
                Write(reader.ReadLine());
            }
        }

        private static List<string> GetRow(Dictionary<string, List<string>> lookup, string key)
        {
            if (lookup.ContainsKey(key))
            {
                var row = lookup[key];
                lookup.Remove(key);

                return row;
            }
            else
                return null;
        }

        private static List<Diff> Compare(List<string> left, List<string> right, string id)
        {
            List<Diff> diffs = GetDiffs(left, right);
            if (diffs.Count != 0)
            {
                Write("Diff: " + id);

                foreach (var d in diffs)
                {
                    Write("\t" + d.ToString());
                }
            }

            return diffs;
        }

        private static List<Diff> GetDiffs(List<string> listLeft, List<string> listRight)
        {
            List<Diff> diffs = new List<Diff>();
            int min = Math.Min(listLeft.Count, listRight.Count);

            for (int i = 0; i < min; i++)
            {
                string l = listLeft[i];
                string r = listRight[i];

                if (l != r)
                {
                    diffs.Add(new Diff(i, l, r));
                }
            }

            for (int i = min; i < listLeft.Count; i++)
            {
                string str = listLeft[i];
                diffs.Add(new Diff(i, str, null));
            }

            for (int i = min; i < listRight.Count; i++)
            {
                string str = listRight[i];
                diffs.Add(new Diff(i, null, str));
            }

            return diffs;
        }

        private static string GetKey(List<string> row)
        {
            int idx = Configs.KeyColumn;

            return row[idx];
        }

        private static void Clean(List<string> row)
        {
            for (int r = 0; r < row.Count; r++)
            {
                string formatter = Configs.GetFormatter(r);
                if (formatter != null)
                {
                    row[r] = Formatters[formatter].Format(row[r]);
                }
            }
        }

        private static Dictionary<string, IFormatter> LoadFormatters()
        {
            Dictionary<string, IFormatter> formatters = new Dictionary<string, IFormatter>();

            formatters.Add(NO_SPACE, new Formatter.RemoveWhiteSpace());

            return formatters;
        }

        private static Configurations LoadConfiguration(string configFile)
        {
            if (configFile == null)
                return new Configurations();

            throw new NotImplementedException();
        }
    }

}
