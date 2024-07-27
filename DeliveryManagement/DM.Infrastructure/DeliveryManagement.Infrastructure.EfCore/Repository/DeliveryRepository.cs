using _01_Framework.Infrastructure;
using DeliveryManagement.Application.Contract.Delivery;
using DeliveryManagement.Domain.DeliveryAgg;
using System.Linq.Expressions;

namespace DeliveryManagement.Infrastructure.EfCore.Repository
{
    public class DeliveryRepository : RepositoryBase<long, Delivery>, IDeliveryRepository
    {
        private readonly DeliveryContext _context;

        public DeliveryRepository(DeliveryContext context) : base(context)
        {
            _context = context;
        }

        public DeliveryViewModel GetDefaultDeliveryBy(long accountId)
        {
            return _context
                .Deliveries
                .Where(x => x.AccountId == accountId & x.DefaultDelivery == true)
                .Select(x => new DeliveryViewModel
                {
                    AccountId = x.AccountId,
                    DefaultAddress = x.DefaultDelivery,
                    Address = x.Address,
                    PostalCode = x.PostalCode
                })
                .FirstOrDefault();
        }

        public List<DeliveryViewModel> List(long accountId)
        {
            return _context
                 .Deliveries
                 .Where(x => x.AccountId == accountId)
                 .Select(x => new DeliveryViewModel
                 {
                     Id = x.Id,
                     Address = x.Address,
                     PostalCode = x.PostalCode,
                     DefaultAddress = x.DefaultDelivery
                 })
                 .OrderBy(x => x.Id)
                 .ToList();
        }

        public void UnSetSetToDefaultAddress(long accountId)
        {
            var delivery = _context
                 .Deliveries
                 .Where(x => x.DefaultDelivery == true & x.AccountId == accountId)
                 .FirstOrDefault();

            if (delivery != null)
                delivery.UnSetDefaultDelivery();
        }
    }
}
