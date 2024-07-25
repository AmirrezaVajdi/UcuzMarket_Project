namespace DeliveryManagement.Application.Contract.Delivery
{
    public class DeliveryViewModel
    {
        public long Id { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public bool DefaultAddress { get; set; }
        public long AccountId { get; set; }
    }
}
