using System.Diagnostics;

namespace Threads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Для 100тыс.
            Console.WriteLine("100 тыс.");
            int[] array1 = new int[100000];
            var rnd = new Random();
            for (int i = 0; i < array1.Length; i++)
            {
                array1[i] = rnd.Next(1, 101);
            }

            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            var sumSunch1 = SumSynch(array1);
            sw1.Stop();
            Console.WriteLine("Синхронный: " + sw1.ElapsedMilliseconds + "мс. " + "Сумма: " + sumSunch1);

            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            var sumThread1 = SumThread(array1);
            sw2.Stop();
            Console.WriteLine("Потоки: " + sw2.ElapsedMilliseconds + "мс. " + "Сумма: " + sumThread1);

            Stopwatch sw3 = new Stopwatch();
            sw3.Start();
            var sumLinq1 = SumLinq(array1);
            sw3.Stop();
            Console.WriteLine("Парал: " + sw2.ElapsedMilliseconds + "мс. " + "Сумма: " + sumLinq1);
            #endregion

            Console.WriteLine();
            Console.WriteLine("1 млн.");

            #region Для 1 млн.
            int[] array2 = new int[1000000];
            var rnd1 = new Random();
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = rnd1.Next(1, 101);
            }

            Stopwatch sw4 = new Stopwatch();
            sw4.Start();
            var sumSynch = SumSynch(array2);
            sw4.Stop();
            Console.WriteLine("Синхронный: " + sw4.ElapsedMilliseconds + "мс. " + "Сумма: " + sumSynch);

            Stopwatch sw5 = new Stopwatch();
            sw5.Start();
            var sumThread2 = SumThread(array2);
            sw5.Stop();
            Console.WriteLine("Потоки: " + sw5.ElapsedMilliseconds + "мс. " + "Сумма: " + sumThread2);

            Stopwatch sw6 = new Stopwatch();
            sw6.Start();
            var sumLinq2 = SumLinq(array2);
            sw6.Stop();
            Console.WriteLine("Парал: " + sw6.ElapsedMilliseconds + "мс. " + "Сумма: " + sumLinq2);
            #endregion

            Console.WriteLine();
            Console.WriteLine("10 млн.");

            #region Для 10 млн.
            int[] array3 = new int[10000000];
            var rnd2 = new Random();
            for (int i = 0; i < array3.Length; i++)
            {
                array3[i] = rnd2.Next(1, 101);
            }

            Stopwatch sw7 = new Stopwatch();
            sw7.Start();
            var sumSynch3 = SumSynch(array3);
            sw7.Stop();
            Console.WriteLine("Синхронный: " + sw7.ElapsedMilliseconds + "мс. " + "Сумма: " + sumSynch3);

            Stopwatch sw8 = new Stopwatch();
            sw8.Start();
            var sumThread3 = SumThread(array3);
            sw8.Stop();
            Console.WriteLine("Потоки: " + sw8.ElapsedMilliseconds + "мс. " + "Сумма: " + sumThread3);

            Stopwatch sw9 = new Stopwatch();
            sw9.Start();
            var sumLinq3 = SumLinq(array3);
            sw9.Stop();
            Console.WriteLine("Парал: " + sw9.ElapsedMilliseconds + "мс. " + "Сумма: " + sumLinq3);
            #endregion
        }

        private static int SumSynch(int[] numbers)
        {
            var sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }

            return sum;
        }

        private static int SumThread(int[] numbers)
        {
            var sum = 0;
            var chunkSize = numbers.Length / 2;

            var threads = new List<Thread>();
            var partialSums = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                int start = i * chunkSize;
                int end = (i + 1) * chunkSize;

                var thread = new Thread(() =>
                {
                    int sum = 0;
                    for (int j = start; j < end; j++)
                    {
                        sum += numbers[j];
                    }
                    partialSums.Add(sum);
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            int result = partialSums.Sum();
            return result;
        }

        private static int SumLinq(int[] numbers)
        {
            return ParallelEnumerable.Range(0, numbers.Length).Sum(i => numbers[i]);
        }
    }
}
