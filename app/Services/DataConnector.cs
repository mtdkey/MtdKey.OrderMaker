using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System;

namespace MtdKey.OrderMaker
{
    public class DataConnector : OrderMakerContext
    {
        public readonly Guid DatabaseId;
        public DataConnector(IHttpContextAccessor _contextAccessor,
            DbContextOptions<OrderMakerContext> options) : base(options)
        {
            var databaseId = (string)_contextAccessor.HttpContext.Items["databaseId"];

            DatabaseId = new Guid(databaseId);
            this.SetDatabase(DatabaseId);
        }
    }
}
