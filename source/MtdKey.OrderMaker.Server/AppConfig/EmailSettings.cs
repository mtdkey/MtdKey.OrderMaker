/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

namespace MtdKey.OrderMaker.AppConfig
{
    public class EmailSettings
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Title { get; set; }
        public string Footer { get; set; }
        public string Host { get; set; }
    }
}
