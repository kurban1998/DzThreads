using System.Diagnostics;

namespace Threads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] sizes = { 100000, 1000000, 10000000 };

            foreach (var size in sizes)
            {
                Console.WriteLine($"Для {size} элементов");
                int[] array = new int[size];
                var rnd = new Random();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = rnd.Next(1, 101);
                }

                Stopwatch swSynch = new Stopwatch();
                swSynch.Start();
                var sumSynch = SumSynch(array);
                swSynch.Stop();

                Console.WriteLine("Синхронный: " + swSynch.ElapsedMilliseconds + "мс. " + "Сумма: " + sumSynch);


                Stopwatch swThread = new Stopwatch();
                swThread.Start();
                var sumThread = SumThread(array);
                swThread.Stop();

                Console.WriteLine("Потоки: " + swThread.ElapsedMilliseconds + "мс. " + "Сумма: " + sumThread);

                Stopwatch swLinq = new Stopwatch();
                swLinq.Start();
                var sumLinq = SumLinq(array);
                swLinq.Stop();

                Console.WriteLine("Параллельно: " + swLinq.ElapsedMilliseconds + "мс. " + "Сумма: " + sumLinq);

                Console.WriteLine();
            }
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
            return numbers.AsParallel().Sum();
        }
    }
}
