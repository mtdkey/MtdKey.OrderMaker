using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder
{
    public interface ITemplateStorage
    {
        Task<string> GetTemplateAsync(EmailTemplate name);
    }
}
