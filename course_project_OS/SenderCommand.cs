using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project_OS
{
    internal class SenderCommand
    {
        static readonly object locker = new object();
        static bool runApplication = true;

        public static void OffApp()
        {
            lock(locker) { runApplication = false; }
        }

        public static void Run()
        {
            while (runApplication)
            {
                Console.WriteLine("Hello Threads");
                Thread.Sleep(1000);
            }
        }
    }
}
