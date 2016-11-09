namespace CSVDiff
{
    public class Diff
    {
        public int Column { get; private set; }
        public string Left { get; private set; }
        public string Right { get; private set; }

        public Diff(int col, string left, string right)
        {
            this.Column = col;
            this.Left = left;
            this.Right = right;
        }

        public override string ToString()
        {
            return string.Format("{0} | Left: {1} | Right: {2}", Column, Left, Right);
        }
    }
}