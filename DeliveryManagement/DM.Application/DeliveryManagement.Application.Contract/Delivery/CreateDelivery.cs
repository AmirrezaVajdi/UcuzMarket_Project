namespace DeliveryManagement.Application.Contract.Delivery
{
    public class CreateDelivery
    {
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public long AccountId { get; set; }
    }
}
