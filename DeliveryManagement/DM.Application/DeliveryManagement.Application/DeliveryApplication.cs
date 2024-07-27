using _01_Framework.Application;
using DeliveryManagement.Application.Contract.Delivery;
using DeliveryManagement.Domain.DeliveryAgg;

namespace DeliveryManagement.Application
{
    public class DeliveryApplication : IDeliveryApplication
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IAuthHelper _helper;

        public DeliveryApplication(IDeliveryRepository deliveryRepository, IAuthHelper helper)
        {
            _deliveryRepository = deliveryRepository;
            _helper = helper;
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

        public DeliveryViewModel GetDefaultDeliveryBy(long accountId)
        {
            return _deliveryRepository.GetDefaultDeliveryBy(accountId);
        }

        public List<DeliveryViewModel> List(long accountId)
        {
            return _deliveryRepository.List(accountId);
        }

        public OperationResult SetToDefaultDelivery(SetToDefaultDelivery command)
        {
            OperationResult operation = new();
            var delivery = _deliveryRepository.Get(command.Id);
            if (delivery == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            _deliveryRepository.UnSetSetToDefaultAddress(_helper.CurrentAccountId());

            delivery.SetToDefaultDelivery();

            _deliveryRepository.SaveChanges();
            return operation.Succeded();
        }
    }
}
