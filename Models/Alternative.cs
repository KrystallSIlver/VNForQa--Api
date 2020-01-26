using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Serializable]
    public class Alternative
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Mark { get; set; }
        public Saaty Saaty { get; set; }
        public int SaatyId { get; set; }

    }
}
