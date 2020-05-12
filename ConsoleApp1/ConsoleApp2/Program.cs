using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = new double[][,] {
                new double[,] { { 381, 68, 651.79, 50, 32550.76, 0, 0, 797, 418, 24, 397, 0, 40, 65, 1, 1, 1, 101 } } };
            double[,] array = new double[,] { { 381, 68, 651.79, 50, 32550.76, 0, 0, 797, 418, 24, 397, 0, 40, 65, 1, 1, 1, 101 } };
            
          
            Console.ReadKey();
        }
        public float[,] Converttofloat(double[,] array)
        {
            var row = array.GetLength(0);
            var clo = array.GetLength(1);
            float[,] arrayfloat = new float[1, 18];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < clo; j++)
                {
                    arrayfloat[i, j] = (float)array[i, j];
                }
            }
            return arrayfloat;
        }
    }
}
