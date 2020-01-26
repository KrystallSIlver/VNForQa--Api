using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using static DataAccessLayer.Enums;

namespace DataAccessLayer.Helpers
{
    public class NormalizationHelper
    {
        public static int MarkToSaatyRange(double mark)
        {
            switch (mark)
            {
                case (int)Rate.VeryLow:
                    return 1;
                case (int)Rate.Low:
                    return 3;
                case (int)Rate.Default:
                    return 5;
                case (int)Rate.High:
                    return 7;
                case (int)Rate.VeryHigh:
                    return 9;
            }
            return 0;
        }

        public static Saaty DefaultSaaty(Alternative[] array)
        {
            double[,] b = new double[array.Length, array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j < array.Length; j++)
                {
                    var c = array[i].Mark - array[j].Mark;
                    if (c > 0)
                    {
                        b[i, j] = c + 1;
                        b[j, i] = 1.0 / (c + 1);
                    }
                    else
                    {
                        b[j, i] = Math.Abs(c) + 1.0;
                        b[i, j] = Math.Round(1 / (Math.Abs(c) + 1.0), 2);
                    }
                }
            }
            return Saaty(array, b);
        }

        public static Saaty KolSaaty(Alternative[] array)
        {
            double[,] b = new double[array.Length, array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j < array.Length; j++)
                {
                    b[i, j] = (array[i].Mark + 1) / (array[j].Mark + 1);
                    b[j, i] = (array[j].Mark + 1) / (array[i].Mark + 1);
                }
            }

            double[] sqrt = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                double aa = 1;
                for (int j = 0; j < array.Length; j++)
                {
                    aa *= b[i, j];
                }
                sqrt[i] = Math.Round(Math.Pow(aa, 1.0 / (array.Length - 1)), 2);
            }

            var sqrtSum = sqrt.Sum();

            List<double> normalized = new List<double>();

            foreach (var s in sqrt)
            {
                normalized.Add((s / sqrtSum));
            }
            var normArray = normalized.ToArray();
            var sum = normArray.Sum();


            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mark = normArray[i];
            }

            List<Row> rows = new List<Row>();

            for (int i = 0; i < array.Length; i++)
            {
                var row = new Row
                {
                    marks = new Mark[array.Length],
                    name = array[i].Name,
                    ei = sqrt[i],
                    omega = array[i].Mark
                };
                for (int j = 0; j < array.Length; j++)
                {
                    row.marks[j] = new Mark
                    {
                        Rate = b[i, j]
                    };
                }
                rows.Add(row);
            }

            var row1 = new Row() { ei = sqrtSum, omega = rows.Sum(x => x.omega) > 0.99999 && rows.Sum(x => x.omega) < 1 ? 1 : rows.Sum(x => x.omega), marks = new Mark[array.Length] };
            for (var a = 0; a < array.Length; a++)
            {
                row1.marks[a] = new Mark() { Rate = -99 };
            }
            rows.Add(row1);

            return new Saaty() { NormalizedSqrts = array.ToList(), Rows = rows };
        }

        public static Saaty GeometryMethod(Alternative[] array)
        {
            double[,] b = new double[array.Length, array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j < array.Length; j++)
                {
                    b[i, j] = (array[i].Mark + 1) / (array[j].Mark + 1);
                    b[j, i] = (array[j].Mark + 1) / (array[i].Mark + 1);
                }
            }

            double[] sqrt = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                double aa = 1;
                for (int j = 0; j < array.Length; j++)
                {
                    aa *= b[i, j];
                }
                sqrt[i] = Math.Round(Math.Pow(aa, 1.0 / (array.Length - 1)), 2);
            }

            var sqrtSum = sqrt.Sum();

            List<double> normalized = new List<double>();

            foreach (var s in sqrt)
            {
                normalized.Add((s / sqrtSum));
            }
            var normArray = normalized.ToArray();
            var sum = normArray.Sum();


            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mark = normArray[i];
            }

            List<Row> rows = new List<Row>();

            for (int i = 0; i < array.Length; i++)
            {
                var row = new Row
                {
                    marks = new Mark[array.Length],
                    name = array[i].Name,
                    ei = sqrt[i],
                    omega = array[i].Mark
                };
                for (int j = 0; j < array.Length; j++)
                {
                    row.marks[j] = new Mark
                    {
                        Rate = b[i, j]
                    };
                }
                rows.Add(row);
            }

            var row1 = new Row() { ei = sqrtSum, omega = rows.Sum(x => x.omega) > 0.99999 && rows.Sum(x => x.omega) < 1 ? 1 : rows.Sum(x => x.omega), marks = new Mark[array.Length] };
            for (var a = 0; a < array.Length; a++)
            {
                row1.marks[a] = new Mark() { Rate = -99 };
            }
            rows.Add(row1);

            return new Saaty() { NormalizedSqrts = array.ToList(), Rows = rows };
        }

        public static Saaty Saaty99(Alternative[] array)
        {
            double[,] b = new double[array.Length, array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j < array.Length; j++)
                {
                    var c = (array[i].Mark - 0.125) - (array[j].Mark - 0.125);
                    if (c > 0)
                    {
                        c++;
                        b[i, j] = c;
                        b[j, i] = 1.0 / c;
                    }
                    else if (c == 0)
                    {
                        b[i, j] = 1.0;
                        b[j, i] = 1.0;
                    }
                    else
                    {
                        c--;
                        b[j, i] = Math.Abs(c);
                        b[i, j] = Math.Round(1 / Math.Abs(c), 2);
                    }
                }
            }
            return Saaty(array, b);


        }


        public static Saaty Saaty(Alternative[] array, double[,] b)
        {
            double[] sqrt = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                double aa = 1;
                for (int j = 0; j < array.Length; j++)
                {
                    aa *= b[i, j];
                }
                sqrt[i] = Math.Round(Math.Pow(aa, 1.0 / array.Length), 2);
            }

            var sqrtSum = sqrt.Sum();

            List<double> normalized = new List<double>();

            foreach (var s in sqrt)
            {
                normalized.Add((s / sqrtSum));
            }
            var normArray = normalized.ToArray();
            var sum = normArray.Sum();


            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mark = normArray[i];
            }

            List<Row> rows = new List<Row>();

            for (int i = 0; i < array.Length; i++)
            {
                var row = new Row
                {
                    marks = new Mark[array.Length],
                    name = array[i].Name,
                    ei = sqrt[i],
                    omega = array[i].Mark
                };
                for (int j = 0; j < array.Length; j++)
                {
                    row.marks[j] = new Mark
                    {
                        Rate = b[i, j]
                    };
                }
                rows.Add(row);
            }
            //if (sum < 1)
            //    for (int i = 0; i < normArray.Length; i++)
            //    {
            //        if (normArray[i] == normArray.Max())
            //        {
            //            normArray[i] += 1 - sum;
            //            break;
            //        }
            //    }
            //if (rows.Select(x => x.omega).Sum() <= 0.99) rows.FirstOrDefault(x=>x.omega == rows.Max(y => y.omega)).omega += 0.1;
            var row1 = new Row() { ei = sqrtSum, omega = rows.Sum(x => x.omega) > 0.99999 && rows.Sum(x => x.omega) < 1 ? 1 : rows.Sum(x => x.omega), marks = new Mark[array.Length] };
            for (var a = 0; a < array.Length; a++)
            {
                row1.marks[a] = new Mark() { Rate = -99 };
            }
            rows.Add(row1);

            //array.OrderByDescending(x => x.Mark);

            return new Saaty() { NormalizedSqrts = array.ToList(), Rows = rows };
        }
    }
}
