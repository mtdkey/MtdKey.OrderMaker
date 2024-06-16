using NPOI.HSSF.UserModel;

namespace MtdKey.OrderMaker.Core
{
    public class HostInfo(string schemaName, string hostName)
    {
        private readonly string schemaName = schemaName;
        private readonly string  hostName = hostName;

        public string Name => $"{schemaName}://{hostName}";

    }
}
