using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project_OS
{
    class CommandParams
    {
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public string Operation { get; set; } = string.Empty;
        public int Timer { get; set; }
        public CommandParams() { }

    }

    class Command
    {
        public long CodeCommand { get; set; }
        public CommandParams CommandParams { get; set; }
        
        public Command(long code, CommandParams commandParams) { CodeCommand = code; CommandParams = commandParams; }
    }

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
