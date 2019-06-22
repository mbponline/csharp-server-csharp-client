using System.Text;

namespace CodeGenerator.Models.Common
{

    public class BlockWriter
    {
        private int currentIndent;

        private StringBuilder stringBuilder;

        public string Result
        {
            get { return this.stringBuilder.ToString(); }
        }

        public BlockWriter(string result = "", int startIndent = 0)
        {
            this.stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(result))
            {
                this.stringBuilder.AppendLine(result);
            }
            this.currentIndent = startIndent;
        }

        public BlockWriter BeginBlock(string text)
        {
            this.stringBuilder.AppendLine(this.Indent(this.currentIndent) + text);
            this.currentIndent += 1;
            return this;
        }

        public BlockWriter WriteLine(string text = "", bool parentIndent = false)
        {
            this.stringBuilder.AppendLine(this.Indent(this.currentIndent - (parentIndent ? 1 : 0)) + text);
            return this;
        }

        public BlockWriter EndBlock(string text = "", bool addEmptyLine = true)
        {
            var endText = string.IsNullOrEmpty(text) ? "};" : text;
            this.currentIndent -= 1;
            this.stringBuilder.AppendLine(this.Indent(this.currentIndent) + endText);
            if (addEmptyLine)
            {
                this.stringBuilder.AppendLine();
            }

            return this;
        }

        private string Indent(int count)
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
