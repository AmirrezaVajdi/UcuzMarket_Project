namespace InventoryManagement.Application.Contract.Inventory
{
    public class DecreaseInventory
    {
        public long PorudctId { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public long OrderId { get; set; }
    }
}
