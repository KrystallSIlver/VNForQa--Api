using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    //public class Saaty
    //{
    //    public Alternative[] Alternatives { get; set; }
    //    public double[,] ComparedAlts { get; set; }
    //    public double[] Sqrt { get; set; }
    //    public double SqrtSum { get; set; }
    //    public Alternative[] NormalizedSqrts { get; set; }
    //}


    public class Row
    {
        public int Id { get; set; }
        public string name = null;
        public Mark[] marks = null;
        public double ei;
        public double omega;
        public int SaatyId { get; set; }
        public Saaty Saaty { get; set; }
    }


    public class Saaty
    {
        public int Id { get; set; }
        public List<Row> Rows { get; set; }
        public List<Alternative> NormalizedSqrts { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
