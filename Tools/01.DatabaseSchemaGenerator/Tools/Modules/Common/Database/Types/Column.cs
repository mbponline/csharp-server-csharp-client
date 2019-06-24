namespace Tools.Modules.Common.Database.Types
{
    public class Column
    {
        public string Table { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Default { get; set; }

        public ulong? MaxLength { get; set; }

        public int IsNullable { get; set; }

        public int IsKey { get; set; }

        public int IsIdentity { get; set; }

        public int IsComputed { get; set; }
    }
}
