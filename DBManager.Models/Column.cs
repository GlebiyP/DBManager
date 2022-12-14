using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        /*public Column(string name)
        {
            Name = name;
        }*/
    }
}
