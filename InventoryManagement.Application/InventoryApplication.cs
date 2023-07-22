using _01_Framework.Application;
using InventoryManagement.Application.Contract.Inventory;

namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        public OperationResult Create(CreateInventory command)
        {
            throw new NotImplementedException();
        }

        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            throw new NotImplementedException();
        }

        public OperationResult Edit(EditInventory command)
        {
            throw new NotImplementedException();
        }

        public EditInventory GetDetails(long id)
        {
            throw new NotImplementedException();
        }

        public OperationResult Increase(IncreaseInventory command)
        {
            throw new NotImplementedException();
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            throw new NotImplementedException();
        }
    }
}
