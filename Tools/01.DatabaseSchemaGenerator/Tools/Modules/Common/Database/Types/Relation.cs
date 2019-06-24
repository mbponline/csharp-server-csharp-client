namespace Tools.Modules.Common.Database.Types
{
    public class Relation
    {
        public string ForeignKeyName { get; set; }

        public string ParentTable { get; set; }

        public string ParentColumnName { get; set; }

        public object ParentColumnId { get; set; }

        public string ReferencedTable { get; set; }

        public string ReferencedColumnName { get; set; }
    }
}
