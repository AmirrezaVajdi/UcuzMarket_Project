namespace InventoryManagement.Application.Contract.Inventory
{
    public class CreateInventory
    {
        public long ProudctId { get; set; }
        public double UnitPrice { get; set; }
    }
    public class EditInventory : CreateInventory
    {
        public long Id { get; set; }
    }
}
