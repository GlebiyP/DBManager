using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public List<Row> Rows { get; set; }

        /*public Table(string name) 
        {
            Name = name;
        }*/
    }
}
