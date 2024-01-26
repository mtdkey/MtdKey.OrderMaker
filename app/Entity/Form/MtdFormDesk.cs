/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFormDesk
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public int ImageSize { get; set; }
        public string ColorFont { get; set; }
        public string ColorBack { get; set; }

        public virtual MtdForm IdNavigation { get; set; }
    }
}
