using BaseLib;
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
        static TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 6543);
        static readonly object locker = new object();
        static bool runApplication = true;

        public static void OffApp()
        {
            lock(locker) { runApplication = false; }
        }

        public static void Run()
        {
            tcpListener.Start();
            
            while (runApplication)
            {
                if (tcpListener.Pending())
                    AnswerToRequest().Wait();
                else
                    Thread.Sleep(100);
            }

            tcpListener.Stop();
        }

        static async Task AnswerToRequest()
        {
            // получаем входящее подключение
            using var tcpClient = await tcpListener.AcceptTcpClientAsync();

            // получаем поток для взаимодействия с агентом
            NetworkStream stream = tcpClient.GetStream();

            // буфер для получения данных
            int SIZE_OF_BUFFER = 512;
            var responseData = new byte[SIZE_OF_BUFFER];
            // StringBuilder для склеивания полученных данных в одну строку
            var response = new StringBuilder();
            int bytes = 0;  // количество полученных байтов
            do
            {
                // получаем данные
                bytes = await stream.ReadAsync(responseData);
                // преобразуем в строку и добавляем ее в StringBuilder
                response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
            }
            while (bytes == SIZE_OF_BUFFER); // пока данные есть в потоке 

            string[] cmd = response.ToString().Split(' ');
            if (cmd[0] == "command")
            {
                if (CommandRepository.TryPop(out Command? command))
                {
                    // конвертируем данные в массив байтов
                    var requestData = Encoding.UTF8.GetBytes(command.ToString());
                    // отправляем данные агенту
                    await stream.WriteAsync(requestData);
                    NoticeRepository.Add(new Notice(command.CodeCommand, "Command accepted for processing"));
                }
            } else if (cmd[0] == "result")
            {
                if (cmd[1] == "error")
                    NoticeRepository.Add(new Notice(-1, cmd[2]));
                else
                    NoticeRepository.Add(new Notice(long.Parse(cmd[1]), cmd[2]));
            }
            //tcpClient.Close();
        }
    }
}
