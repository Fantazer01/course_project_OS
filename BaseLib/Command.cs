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
                .Append(CommandParams.Number1).Append(' ')
                .Append(CommandParams.Number2).Append(' ')
                .Append(CommandParams.Operation).Append(' ')
                .Append(CommandParams.Timer);
            return requestMessageStrBuild.ToString();
        }
    }
}