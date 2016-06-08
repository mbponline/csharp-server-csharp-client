using System.Text;

namespace CodeGenerator.Models.Common
{

    public class BlockWriter
    {
        private int currentIndent;

        private StringBuilder lines;

        public BlockWriter(string result = "", int startIndent = 0)
        {
            this.lines = new StringBuilder();
            if (!string.IsNullOrEmpty(result))
            {
                this.lines.AppendLine(result);
            }
            this.currentIndent = startIndent;
        }

        public BlockWriter BeginBlock(string text)
        {
            this.lines.AppendLine(this.WriteIndent(this.currentIndent) + text);
            this.currentIndent += 1;
            return this;
        }

        public BlockWriter WriteLine(string text = "", bool parentIndent = false)
        {
            this.lines.AppendLine(this.WriteIndent(this.currentIndent - (parentIndent ? 1 : 0)) + text);
            return this;
        }

        public BlockWriter EndBlock(string text = "", bool addEmptyLine = true)
        {
            var endText = string.IsNullOrEmpty(text) ? "};" : text;
            this.currentIndent -= 1;
            this.lines.AppendLine(this.WriteIndent(this.currentIndent) + endText);
            if (addEmptyLine)
            {
                this.lines.AppendLine();
            }

            return this;
        }

        public override string ToString()
        {
            return this.lines.ToString();
        }

        private string WriteIndent(int count)
        {
            var indentSize = 4; // numarul de caractere pentru un indent
            var space = " ";
            var tab = new StringBuilder();
            for (var i = 0; i < indentSize * count; i++)
            {
                tab.Append(space);
            }

            return tab.ToString();
        }
    }

}
