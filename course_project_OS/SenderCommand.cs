using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            var task = Method();
            task.Wait();

            /*
            while (runApplication)
            {
                AnswerToRequest();
                Thread.Sleep(500);
            }*/
        }

        static async Task Method()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6543);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);   // связываем с локальной точкой ipPoint
            socket.Listen(1000);    // запускаем прослушивание
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
            // получаем входящее подключение
            using Socket client = await socket.AcceptAsync();
            // получаем адрес клиента
            Console.WriteLine($"Адрес подключенного клиента: {client.RemoteEndPoint}");
            // получаем конечную точку, с которой связан сокет
            Console.WriteLine(socket.LocalEndPoint); // 0.0.0.0:8888
        }

        private static void AnswerToRequest()
        {
            if (CommandRepository.TryPop(out Command? command))
            {
                Console.WriteLine("Hello Threads");
                NoticeRepository.Add(new Notice(command.CodeCommand, "Command accepted for processing"));
            }
        }
    }
}
