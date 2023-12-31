﻿
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
        static readonly NoticeRepository Instance = new NoticeRepository();
        static readonly object locker = new object();

        List<Notice> notices = new List<Notice>();

        public static void Add(Notice notice)
        {
            lock (locker) { Instance.notices.Add(notice); }
        }

        public static List<Notice> GetNotices()
        {
            List<Notice> list;
            lock (locker) 
            {
                list = Instance.notices;
                Instance.notices = new List<Notice>();
            }
            return list;
        }

    }
}
