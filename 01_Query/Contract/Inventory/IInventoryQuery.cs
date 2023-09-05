namespace _01_Query.Contract.Inventory
{
    public interface IInventoryQuery
    {
        public StockStatus CheckStock(IsInStock command);
    }
}
