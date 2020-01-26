using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public double Rate { get; set; } = 0;
        public Row Row { get; set; }
    }
}
