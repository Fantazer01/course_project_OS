﻿using BaseLib;

namespace course_project_OS
{
    internal class CommandRepository
    {
        static readonly CommandRepository Instance = new CommandRepository();
        static long counter = 0;
        static readonly object locker = new object();

        readonly List<Command> Commands = new List<Command>();

        public static long Add(CommandParams commandParams)
        {
            lock (locker)
            {
                ++counter;
                Instance.Commands.Add(new Command(counter, commandParams));
            }
            return counter;
        }

        public static bool Empty()
        {
            lock (locker)
            {
                return Instance.Commands.Count == 0;
            }
        }

        public static bool TryPop(out Command? command)
        {
            bool result;
            lock (locker) 
            {
                result = Instance.Commands.Count != 0;
                if (result)
                {
                    command = Instance.Commands.First();
                    Instance.Commands.RemoveAt(0);
                }
                else
                    command = null;
            }
            
            return result;
        }
    }
}
