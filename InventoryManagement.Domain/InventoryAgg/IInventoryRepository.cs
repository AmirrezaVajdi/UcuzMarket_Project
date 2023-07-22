using _01_Framework.Domain;
using InventoryManagement.Application.Contract.Inventory;

namespace InventoryManagement.Domain.InventoryAgg
{
    public interface IInventoryRepository : IRepository<long, Inventory>
    {
        EditInventory GetDetails(long id);
        Inventory Get(long productId);
        List<InventoryViewModel> Search(InventorySearchModel searchModel);
    }
}
