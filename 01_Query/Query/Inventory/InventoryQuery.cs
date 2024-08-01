using _01_Query.Contract.Inventory;
using InventoryManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_Query.Query
{
    public class InventoryQuery : IInventoryQuery
    {
        private readonly InventoryContext _inventoryContext;
        private readonly ShopContext _shopContext;

        public InventoryQuery(InventoryContext inventoryContext, ShopContext shopContext)
        {
            _inventoryContext = inventoryContext;
            _shopContext = shopContext;
        }

        public StockStatus CheckStock(IsInStock command)
        {
            var inventory = _inventoryContext.Inventory.FirstOrDefault(x => x.ProductId == command.ProductId);
            var res = inventory.CalculateCurrentCount();
            if (inventory == null || inventory.CalculateCurrentCount() < command.Count)
            {
                var product = _shopContext.Products.Select(x => new { x.Id, x.Name }).FirstOrDefault(x => x.Id == command.ProductId);
                return new()
                {
                    InStock = false,
                    ProductName = product.Name
                };
            }
            return new() { InStock = true };
        }
    }
}
