using System.Diagnostics.CodeAnalysis;

namespace DeliveryManagement.Application.Contract.Delivery
{
    public class EditDelivery : CreateDelivery
    {
        [NotNull]
        public long Id { get; set; }
    }
}
