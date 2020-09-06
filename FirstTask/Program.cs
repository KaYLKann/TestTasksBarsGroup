using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestTasksBarsGroup.FirstTask;

namespace FirstTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var enumerable = Enumerable.Range(1, 100);
            Parallel.ForEach(enumerable, i =>
            {
                if (i % 2 == 0)
                {
                    ServerStaticClass.Instance.AddToCount(i);
                    Console.WriteLine($"[W]: I #:{i} write value");
                    
                    return;
                }

                Console.WriteLine($"[R]: I #:{i} read value: {ServerStaticClass.Instance.GetCount()}");
            });


            var sum = enumerable.Where(i => i % 2 == 0).Sum();

            Console.WriteLine($"Single thread summing result :{sum},  safetyCounter result: {ServerStaticClass.Instance.GetCount()}");
        }
    }
}