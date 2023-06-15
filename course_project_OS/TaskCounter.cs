
namespace course_project_OS
{
    internal class TaskCounter
    {
        static int counter = 0;
        static readonly object counterLock = new object();
        public static int Counter() { lock(counterLock) {  return counter; } }
        public static bool Empty() { lock (counterLock) { return counter == 0; } }
        public static void Increase() { lock(counterLock) { ++counter; } }
        public static void Decrease() { lock(counterLock) { --counter; } }
    }
}
