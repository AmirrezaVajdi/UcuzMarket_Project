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

        public List<DeliveryViewModel> List(long accountId)
        {
            return _context
                 .Deliveries
                 .Where(x => x.AccountId == accountId)
                 .Select(x => new DeliveryViewModel
                 {
                     Address = x.Address,
                     PostalCode = x.PostalCode
                 })
                 .ToList();
        }
    }
}
