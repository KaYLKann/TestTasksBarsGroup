using System;
using System.Threading;

namespace SecondTask
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            EventHandler h = new EventHandler(Target);
            var ac = new AsyncCaller(h);
            bool completedIsOk = ac.Invoke(5000, null, EventArgs.Empty);

            Console.WriteLine($"Returned state is: {completedIsOk}");
        }
        
        private static void Target(object sender, EventArgs e)
        {
            Console.WriteLine("Target: Start wait...");
            
            Thread.Sleep(600);
            
            Console.WriteLine("Target: End wait!");
        }
    }
}