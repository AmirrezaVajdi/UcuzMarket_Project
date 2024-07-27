using _01_Framework.Domain;
using DeliveryManagement.Application.Contract.Delivery;

namespace DeliveryManagement.Domain.DeliveryAgg
{
    public interface IDeliveryRepository : IRepository<long, Delivery>
    {
        List<DeliveryViewModel> List(long accountId);
        void UnSetSetToDefaultAddress(long accountId);
        DeliveryViewModel GetDefaultDeliveryBy(long accountId);
    }
}
