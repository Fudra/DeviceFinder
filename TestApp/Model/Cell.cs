using System.Text;

namespace TestApp.Model
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("X: " + X);
            sb.Append(", Y: " + Y);
            return sb.ToString();
        }
    }
}