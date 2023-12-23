using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Diagnostics;


namespace MyNameSpace
{
    class Vector
    {
        private List<int> vector;

        public Vector(List<int> vector)
        {
            this.vector = vector;
        }

        public List<int> getVector() { return this.vector; }
        
        public void setVector(List<int> vector) {  this.vector = vector; }

        public static int scalarMult(Vector vector1, Vector vector2)
        {
            int ln = Math.Min(vector1.vector.Count, vector2.vector.Count);
            int scalar = 0;
            for (int i = 0; i < ln; i++)
            {
                scalar += (vector1.vector[i] * vector2.vector[i]);
            }
            return scalar;
        }

        public static int scalarMultAsync(Vector vector1, Vector vector2, int threads)
        {
            int ln = Math.Min(vector1.vector.Count, vector2.vector.Count);
            int scalar = 0;
            int chunkSize = threads;
            Task<int>[] tasks = new Task<int>[chunkSize];

            for (int i = 0; i < chunkSize; i++)
            {
                int start = i * ln / chunkSize;
                int end = (i == chunkSize - 1) ? ln : (i + 1) * ln / chunkSize;
                tasks[i] = Task<int>.Factory.StartNew(state =>
                {
                    var range = (Tuple<int, int>)state;
                    int localScalar = 0;
                    for (int j = range.Item1; j < range.Item2; j++)
                    {
                        localScalar += (vector1.vector[j] * vector2.vector[j]);
                    }
                    return localScalar;
                }, Tuple.Create(start, end));
            }

            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                scalar += task.Result;
            }

            return scalar;
        }
    }

    class Program
    {
        static List<int> GenerateRandomList(int length)
        {
            Random random = new Random();
            List<int> randomList = new List<int>();

            for (int i = 0; i < length; i++)
            {
                randomList.Add(random.Next(1, 100));
            }

            return randomList;
        }

        static void Main(string[] args)
        {
            int[] thread = new int[] { 2, 6, 12 };

            List<int> list1_1 = GenerateRandomList(1000000);
            List<int> list1_2 = GenerateRandomList(1000000);
            List<int> list2_1 = GenerateRandomList(5000000);
            List<int> list2_2 = GenerateRandomList(5000000);
            List<int> list3_1 = GenerateRandomList(10000000);
            List<int> list3_2 = GenerateRandomList(10000000);
            List<int> list4_1 = GenerateRandomList(50000000);
            List<int> list4_2 = GenerateRandomList(50000000);
            List<int> list5_1 = GenerateRandomList(100000000);
            List<int> list5_2 = GenerateRandomList(100000000);
            foreach (int threads in thread)
            {
                Console.WriteLine($"Number of threads: {threads}\n");

                Vector v1 = new Vector(list1_1);
                Vector v2 = new Vector(list1_2);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Vector.scalarMult(v1, v2);
                stopwatch.Stop();
                Console.WriteLine($"Syncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds} ms");
                stopwatch.Restart();
                Vector.scalarMultAsync(v1, v2, threads);
                stopwatch.Stop();
                Console.WriteLine($"Asyncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds} ms\n");

                v1.setVector(list2_1);
                v2.setVector(list2_2);
                stopwatch.Restart();
                Vector.scalarMult(v1, v2);
                stopwatch.Stop();
                Console.WriteLine($"Syncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds} ms");
                stopwatch.Restart();
                Vector.scalarMultAsync(v1, v2, threads);
                stopwatch.Stop();
                Console.WriteLine($"Asyncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms\n");

                v1.setVector(list3_1);
                v2.setVector(list3_2);
                stopwatch.Restart();
                Vector.scalarMult(v1, v2);
                stopwatch.Stop();
                Console.WriteLine($"Syncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms");
                stopwatch.Restart();
                Vector.scalarMultAsync(v1, v2, threads);
                stopwatch.Stop();
                Console.WriteLine($"Asyncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms\n");

                v1.setVector(list4_1);
                v2.setVector(list4_2);
                stopwatch.Restart();
                Vector.scalarMult(v1, v2);
                stopwatch.Stop();
                Console.WriteLine($"Syncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms");
                stopwatch.Restart();
                Vector.scalarMultAsync(v1, v2, threads);
                stopwatch.Stop();
                Console.WriteLine($"Asyncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms\n");

            
                v1.setVector(list5_1);
                v2.setVector(list5_2);
                stopwatch.Restart();
                Vector.scalarMult(v1, v2);
                stopwatch.Stop();
                Console.WriteLine($"Syncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms");
                stopwatch.Restart();
                Vector.scalarMultAsync(v1, v2, threads);
                stopwatch.Stop();
                Console.WriteLine($"Asyncronic scalar vector multiplying with size of vectors {v1.getVector().Count}: {stopwatch.Elapsed.TotalMilliseconds}  ms\n");
                stopwatch.Restart();
            }
        }
    }
}








