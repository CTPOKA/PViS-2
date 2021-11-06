using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _102
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите D1:");
                int D1 = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите S1:");
                int S1 = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите G:");
                int G = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите число обрабатываемых потоков:");
                int threads = int.Parse(Console.ReadLine());
                var M1 = RandomM(D1, S1);
                var M2 = RandomM(D1 - G, S1 - G);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                SubM(M1, M2, threads);
                stopWatch.Stop();
                Console.WriteLine("Время обработки: {0} мс\n", stopWatch.Elapsed.TotalMilliseconds);
            }
        }
        //Генерация случайной матрицы с указанной размерностью
        static int[,] RandomM(int D, int S)
        {
            Random rand = new Random();
            int X = (int)Math.Pow(2, D);
            int Y = (int)Math.Pow(2, S);
            int[,] M = new int[X, Y];
            Parallel.For(0, X, x =>
            {
                for (int y = 0; y < Y; y++)
                {
                    M[x, y] = rand.Next(0, 100);
                }
            });
            return M;
        }
        // Кроссмасштабное вычитание двух матриц
        static int[,] SubM(int[,] M1, int[,] M2, int threads = 4)
        {
            int[,] M3 = M1;
            int g = M1.GetLength(0) / M2.GetLength(0);
            Parallel.For(0, M1.GetLength(0), new ParallelOptions { MaxDegreeOfParallelism = threads }, x =>
            {
                for (int y = 0; y < M1.GetLength(1); y++)
                {
                    M3[x, y] -= M2[x / g, y / g];
                }
            });
            return M3;
        }
        //Вывод матрицы на экран
        static void PrintM(int[,] M)
        {
            string str = "";
            for (int x = 0; x < M.GetLength(0); x++)
            {
                str += " | ";
                for (int y = 0; y < M.GetLength(1); y++)
                {
                    for (int k = 0; k < 2 - Math.Log10(Math.Abs(M[x, y])); k++)
                        str += " ";
                    if (M[x, y] > 0)
                        str += " ";
                    str += M[x, y];
                    str += y < M.GetLength(1) - 1 ? ", " : " |\n";
                }
            }
            Console.WriteLine(str);
        }
    }
}
