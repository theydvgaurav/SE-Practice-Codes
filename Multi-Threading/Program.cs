using System;
using System.Threading;

namespace Multi_Threading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // using two threads 
            // t1 -> used to invoke countUp()
            // t2 -> used to invoke countDown()
            // Thread t1 = new Thread(countUp);
            // Thread t2 = new Thread(countDown);
            // t1.Start();
            // t2.Start();

            List<int> numbers = null;
            int? a = null;

            (numbers ??= new List<int>()).Add(5);
            Console.WriteLine(string.Join(" ", numbers));  // output: 5
            var b = a ?? 70;
            numbers.Add(a ??= 0);
            Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
            Console.WriteLine(a);  // output: 0
            Console.WriteLine(b);  // output: 0

            // countUp();
            // countDown();
            Console.ReadLine();
        }

        static void countUp()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Timer 1: {i} seconds");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Timer 1 Finished");

        }

        static void countDown()
        {
            for (int i = 100; i >= 0; i--)
            {
                Console.WriteLine($"Timer 2: {i} seconds");
                Thread.Sleep(1000);

            }

            Console.WriteLine("Timer 2 Finished");
        }
    }
}