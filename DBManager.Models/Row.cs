using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Models
{
    public class Row
    {
        public int Id { get; set; }
        public List<string> Values { get; set; } = new List<string>();
    }
}
