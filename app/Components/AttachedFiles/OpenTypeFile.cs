using System;
using System.Linq;

namespace MtdKey.OrderMaker.Components.AttachedFiles
{
    public static class OpenTypeFile
    {
        private static string[] List => [
            "application/pdf",
            "image/jpeg",
            "image/png",
        ];

        public static bool CheckType(string type) => List.Contains(type);
    }
}
