using BaseLib;
using System.Text;

namespace DataGenerator
{
    enum Operation
    {
        Plus = 0,
        Minus = 1,
        Multiplication = 2,
        Division = 3
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Сколько требуется сгенерировать записей:");
            if (!int.TryParse(Console.ReadLine(), out int n))
                return;

            List<CommandParams> commands = new List<CommandParams>();

            for (int i = 0; i < n; ++i)
                commands.Add(GenereteCommand());

            WriteCommand(commands);
        }

        static CommandParams GenereteCommand()
        {
            var rand = new Random();

            Operation op = (Operation)rand.Next(4);
            return new CommandParams()
            {
                Number1 = rand.NextDouble() * 1000,
                Number2 = rand.NextDouble() * 1000,
                Operation = op switch
                {
                    Operation.Plus => "+",
                    Operation.Minus => "-",
                    Operation.Multiplication => "*",
                    Operation.Division => "/",
                    _ => throw new NotImplementedException(),
                },
                Timer = 0
            };
        }

        static void WriteCommand(List<CommandParams> commands)
        {
            FileStream fileStream = File.Create($"data{commands.Count}.txt");
            foreach (var cmd in commands)
            {
                byte[] buffer = Encoding.Default.GetBytes("calc\n" + cmd.ToString() + "\n");
                // запись массива байтов в файл
                fileStream.WriteAsync(buffer, 0, buffer.Length).Wait();
            }

            fileStream.Close();
        }
    }
}