using BaseLib;
using System.Net.Sockets;
using System.Text;

namespace Agent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Укажите количество агентов:");
            if (!int.TryParse(Console.ReadLine(), out int n))
                return;

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < n; ++i)
            {
                Thread thread = new Thread(AgentHandler);
                thread.Name = i.ToString();
                thread.Start();
                threads.Add(thread);
            }

        }

        static AutoResetEvent waitHandler = new AutoResetEvent(true);  // объект-событие

        static void AgentHandler()
        {
            while (true)
            {
                string response;
                try
                {
                    var task = GetResponse();
                    response = task.Result;

                    if (response == null || response == string.Empty)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        string resultOfCalculation = CommandProcessing(response);
                        var task2 = SendResult(resultOfCalculation);
                        task2.Wait();
                        Console.WriteLine(resultOfCalculation);
                    }
                }
                catch
                {
                    //break;
                }
            }
        }

        static async Task<string> GetResponse()
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", 6543);
            var stream = tcpClient.GetStream();

            var requestData = Encoding.UTF8.GetBytes("command");
            // отправляем данные серверу
            await stream.WriteAsync(requestData);

            // буфер для получения данных
            int SIZE_OF_BUFFER = 512;
            var responseData = new byte[SIZE_OF_BUFFER];
            // StringBuilder для склеивания полученных данных в одну строку
            var response = new StringBuilder();
            int bytes;  // количество полученных байтов
            do
            {
                // получаем данные
                bytes = await stream.ReadAsync(responseData);
                // преобразуем в строку и добавляем ее в StringBuilder
                response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
            }
            while (bytes == SIZE_OF_BUFFER); // пока данные есть в потоке 
            tcpClient.Close();
            return response.ToString();
        }

        static string CommandProcessing(string response)
        {
            if (TryParseParams(response, out var command))
            {
                return CalculateCommand(command);
            }
            return "result error: Incorrect format of parameters";
        }

        static bool TryParseParams(string line, out Command command)
        {
            command = new Command();
            string[] param = line.Split(' ');
            if (param.Length != 5)
            {
                Console.WriteLine("Ошибка. Некорректное количество параметров.");
                return false;
            }

            try
            {
                command.CodeCommand = long.Parse(param[0]);
                command.CommandParams.Number1 = double.Parse(param[1]);
                command.CommandParams.Number2 = double.Parse(param[2]);
                if (param[3] == "+" || param[3] == "-" || param[3] == "*" || param[3] == "/")
                    command.CommandParams.Operation = param[3];
                else
                    throw new Exception("Incorrect operation parameter in command");
                command.CommandParams.Timer = int.Parse(param[4]);
            }
            catch
            {
                Console.WriteLine("Ошибка. Некорректный формат одного из параметров.");
                return false;
            }

            return true;
        }

        static string CalculateCommand(Command command)
        {
            Thread.Sleep(1000);
            if (command.CommandParams.Operation == "+")
                return $"result {command.CodeCommand} {command.CommandParams.Number1 + command.CommandParams.Number2}";
            else if (command.CommandParams.Operation == "-")
                return $"result {command.CodeCommand} {command.CommandParams.Number1 - command.CommandParams.Number2}";
            else if (command.CommandParams.Operation == "*")
                return $"result {command.CodeCommand} {command.CommandParams.Number1 * command.CommandParams.Number2}";
            else if (command.CommandParams.Operation == "/" && command.CommandParams.Number2 != 0)
                return $"result {command.CodeCommand} {command.CommandParams.Number1 / command.CommandParams.Number2}";
            return "result error: division by zero";
        }

        static async Task SendResult(string resultOfCalculation)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", 6543);
            NetworkStream stream = tcpClient.GetStream();
            // конвертируем данные в массив байтов
            var requestData = Encoding.UTF8.GetBytes(resultOfCalculation);
            // отправляем данные серверу
            await stream.WriteAsync(requestData);
        }
    }
}