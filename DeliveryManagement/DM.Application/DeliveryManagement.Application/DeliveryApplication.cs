using _01_Framework.Application;
using DeliveryManagement.Application.Contract.Delivery;
using DeliveryManagement.Domain.DeliveryAgg;

namespace DeliveryManagement.Application
{
    public class DeliveryApplication : IDeliveryApplication
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryApplication(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public OperationResult Create(CreateDelivery command)
        {
            OperationResult operation = new();
            var role = new Delivery(command.Address, command.PostalCode, command.AccountId);

            _deliveryRepository.Create(role);
            _deliveryRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditDelivery command)
        {
            OperationResult operation = new();

            var delivery = _deliveryRepository.Get(command.Id);
            if (delivery == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            delivery.Edit(command.Address, command.PostalCode);

            _deliveryRepository.SaveChanges();
            return operation.Succeded();
        }

        public List<DeliveryViewModel> List(long accountId)
        {
            return _deliveryRepository.List(accountId);
        }
    }
}
