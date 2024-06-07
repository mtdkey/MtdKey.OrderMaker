using Microsoft.EntityFrameworkCore;
using System;

namespace MtdKey.OrderMaker.Entity
{
    public static class ContextExtensions
    {
        public static void SetDatabase(this OrderMakerContext dbContext, Guid databaseId)
        {
            if (databaseId == Guid.Empty) return;
            var connectionString = Program.TemplateConnectionStaring.Replace("{database}", databaseId.ToString());
            dbContext.Database.SetConnectionString(connectionString);
        }
    }
}
