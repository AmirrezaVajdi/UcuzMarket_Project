using InventoryManagement.Application.Contract.Inventory;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public bool ReduceFromInventory(List<OrderItem> items)
        {
            List<ReduceInventory> command = new();
            foreach (var orderItem in items)
            {
                ReduceInventory item = new(orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId);
                command.Add(item);
            }

            return _inventoryApplication.Reduce(command).isSuccedded;
        }
    }
}