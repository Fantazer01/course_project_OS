using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project_OS
{
    class Notice
    {
        public long CodeCommand { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public Notice(long code, string message)
        {
            CodeCommand = code;
            Message = message;
        }
    }

    internal class NoticeRepository
    {
        static NoticeRepository Instance = new NoticeRepository();
        List<Notice> notices = new List<Notice>();

        public static void Add(Notice notice) => Instance.notices.Add(notice);

        public static List<Notice> GetNotices()
        {
            List<Notice> list = Instance.notices;
            Instance.notices = new List<Notice>();
            return list;
        }

    }
}
