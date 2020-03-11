using System.Collections.Generic;
using System.Linq;

namespace Libraries
{

    public class TableMap : Dictionary<TableID, List<IExprValue>>
    {
        public TableMap() : base()
        {
            Add(new TableID(1, 'A'),
                Enumerable.Range(0, 32).Select(i => (IExprValue)new IntegerValue(i)).ToList());
            Add(new TableID(5, 'A'),
                Enumerable.Range(0, 7).Select(i => (IExprValue)new IntegerValue(i)).ToList());
            Add(new TableID(6, 'A'),
                new List<IExprValue> { new ConstStringValue("0"), new ConstStringValue("1") });

            //Add(new TableID(4, 'A'),
            //    Enumerable
        }
    }

    public class TableID
    {
        public int index { get; }
        public char section { get; }
        public TableID(int index, char section)
        {
            this.index = index;
            this.section = section;
        }
        public override int GetHashCode()
        {
            return index.GetHashCode() ^ section.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is TableID)
            {
                TableID tableID = obj as TableID;
                return index == tableID.index
                    && section == tableID.section;
            }
            return false;
        }
    }
}