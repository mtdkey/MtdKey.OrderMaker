using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using MtdKey.OrderMaker.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
