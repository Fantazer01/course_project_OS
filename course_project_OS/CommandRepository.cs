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
        static CommandRepository Instance = new CommandRepository();
        List<Command> Commands = new List<Command>();
        static long counter = 0;

        public static long Add(CommandParams commandParams)
        {
            ++counter;
            Instance.Commands.Add(new Command(counter, commandParams));
            return counter;
        }

        public static bool Empty() => Instance.Commands.Count == 0;

        public static Command Pop()
        {
            Command command = Instance.Commands.First();
            Instance.Commands.RemoveAt(0);
            return command;
        }
    }
}
