using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public interface IStoreService
    {
        Task CreateStoreAsync(StorePostRequest storeRequest);
        Task SaveStoreAsync(StorePostRequest storeRequest);
        Task<RequestResult> GetDocsBySQLRequestAsync(StoreDocRequest docRequest);
        Task<DocModel> CreateEmptyDocModelAsync(StoreDocRequest request);
    }
}
