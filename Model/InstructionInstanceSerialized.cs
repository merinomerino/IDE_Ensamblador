using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicMenu
{
    public class InstructionInstanceSerialized : InstructionInstance
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Str { get; set; }
        
        public InstructionInstanceSerialized() : base(null, null, null)
        {

        }
        public InstructionInstanceSerialized(string str): base(null, null, null)
        {
            this.Str = str;
        }
        
        public override string ToString()
        {
            return Str;
        }
    }
}
