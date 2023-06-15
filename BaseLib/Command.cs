using System.Text;

namespace BaseLib
{
    public class CommandParams
    {
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public string Operation { get; set; } = string.Empty;
        public int Timer { get; set; }
        public CommandParams() { }

        public override string ToString()
        {
            StringBuilder cmdParamsStrBuild = new StringBuilder();
            cmdParamsStrBuild.Append(Number1).Append(' ')
                .Append(Number2).Append(' ')
                .Append(Operation).Append(' ')
                .Append(Timer);
            return cmdParamsStrBuild.ToString();

        }
    }

    public class Command
    {
        public long CodeCommand { get; set; }
        public CommandParams CommandParams { get; set; }

        public Command() { CommandParams = new CommandParams(); }

        public Command(long code, CommandParams commandParams) { CodeCommand = code; CommandParams = commandParams; }

        public override string ToString()
        {
            StringBuilder requestMessageStrBuild = new StringBuilder();
            requestMessageStrBuild.Append(CodeCommand).Append(' ')
                .Append(CommandParams);
            return requestMessageStrBuild.ToString();
        }
    }
}