using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Saaty[] Saaties { get; set; }
    }
}
