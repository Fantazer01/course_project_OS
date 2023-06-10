using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project_OS
{
    internal class NoticeProcessing
    {
        public static void NoticeDialog()
        {
            List<Notice> list = NoticeRepository.GetNotices();
            if (list.Count > 0)
            {
                foreach (Notice notice in list)
                {
                    Console.WriteLine($"{notice.DateTime}|Command code({notice.CodeCommand})|Message: {notice.Message}");
                }
            } else
            {
                Console.WriteLine("Новых уведомлений нет.");
            }
            
        }
    }
}
