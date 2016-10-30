using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad6
{
    class Program
    {
        static void Main(string[] args)
        {
            Method();
            Console.ReadLine();
        }
        private static object _lock = new object();
        static void Method()
        {
            int counter = 0;
            lock (_lock)
            {
                Parallel.For(0, 100, (i) =>
                {
                    counter += 1;
                });
                Console.WriteLine(" Counter should be 100. Counter is {0}", counter);
            }
        }


    }
}
