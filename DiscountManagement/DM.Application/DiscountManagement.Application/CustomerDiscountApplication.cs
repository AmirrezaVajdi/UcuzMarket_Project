using _01_Framework.Application;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagment.Application.Contract.CustomerDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustomerDiscountRepository _customerDiscountRepository;

        public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository)
        {
            _customerDiscountRepository = customerDiscountRepository;
        }

        public OperationResult Define(DefineCustomerDiscount command)
        {
            OperationResult operation = new();
            if (_customerDiscountRepository.Exsists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();

            var customerDiscount = new CustomerDiscount(command.ProductId, command.DiscountRate, startDate, endDate, command.Reason);

            _customerDiscountRepository.Create(customerDiscount);
            _customerDiscountRepository.SaveChanges();
            return operation.Succeded();
        }


        public OperationResult Edit(EditCustomerDiscount command)
        {
            OperationResult operation = new();
            var customerDiscount = _customerDiscountRepository.Get(command.Id);
            if (customerDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_customerDiscountRepository.Exsists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();

            customerDiscount.Edit(command.ProductId, command.DiscountRate, startDate, endDate, command.Reason);
            _customerDiscountRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditCustomerDiscount GetDetails(long id)
        {
            return _customerDiscountRepository.GetDetails(id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
            return _customerDiscountRepository.Search(searchModel);
        }
    }
}
