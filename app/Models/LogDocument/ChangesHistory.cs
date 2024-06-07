/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

namespace MtdKey.OrderMaker.Models.LogDocument
{
    public class ChangesHistory
    {
        public string CreateByUser { get; set; } = "Unknown";
        public string CreateByTime { get; set; } = "Unknown";
        public string LastEditedUser { get; set; } = "Unknown";
        public string LastEditedTime { get; set; } = "Unknown";
    }

}

