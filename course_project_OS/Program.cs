using System.Diagnostics;

namespace course_project_OS
{
    internal class Program
    {
        static readonly string welcome = "Добро пожаловать!\nВы находитесь в программе курсового проекта по OS.\nФункционал данной программы позволяет выполнить операции в распределенной агентной системе";

        static readonly string introduction = "Введите одну из указаных комманд\n"
                                   + "calc - для выполнения арифметических вычислений над двумя числами\n"
                                   + "notice - для получения уведомлений с результатом выполнения заданых операций\n"
                                   + "exit - для выхода из программы\n";
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Console.WriteLine(welcome);

            Thread senderCommand = new Thread(SenderCommand.Run);
            senderCommand.Name = "Sender";
            senderCommand.Start();

            while (true)
            {
                Console.WriteLine(introduction);
                string? command = Console.ReadLine();
                if (command == null || command == "exit")
                    break;
                ProcessingCommand(command);
            }

            while (!TaskCounter.Empty())
                Thread.Sleep(1000);

            SenderCommand.OffApp();

            stopWatch.Stop();

            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);
        }

        static void ProcessingCommand(string command)
        {
            if (command == "calc")
                CalcProcessing.CalcDialog();
            else if (command == "notice")
                NoticeProcessing.NoticeDialog();
            else
                Console.WriteLine("Вы ввели некоректную команду, попробуйте заново.\n");
        }

    }
}