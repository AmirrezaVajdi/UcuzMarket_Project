using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Application.Contract.Inventory
{
    public class ReduceInventory
    {
        public long InventoryId { get; set; }
        public long PorudctId { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public int Count { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Description { get; set; }
        public long OrderId { get; set; }

        public ReduceInventory()
        {
            
        }

        public ReduceInventory(long porudctId, int count, string description, long orderId)
        {
            PorudctId = porudctId;
            Count = count;
            Description = description;
            OrderId = orderId;
        }
    }
}
