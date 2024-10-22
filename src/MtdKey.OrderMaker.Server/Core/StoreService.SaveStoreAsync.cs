using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public partial class StoreService : IStoreService
    {

        public async Task SaveStoreAsync(StorePostRequest storeRequest)
        {

            var store = await context.MtdStore.FindAsync(storeRequest.StoreId);
            if (store == null) { return; }

            await AddStoreItemsAsync(storeRequest, store);

            await context.SaveChangesAsync();
        }


    }
}
