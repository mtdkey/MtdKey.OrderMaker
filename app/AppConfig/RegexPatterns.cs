using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.AppConfig
{
    public static class RegexPatterns
    {
        public static string Email => "^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$";
        public static string Phone => "[+][0-9]{1}\\s[0-9]{3}\\s[0-9]{3}\\s[0-9]{4}";
        public static string Numbers => "^(?:[1-9][0-9]*|0)$";
    }
}
