using _01_Framework.Application;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using DiscountManagment.Application.Contract.ColleagueDiscount;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DiscountManagement.Application
{
    public class ColleagueDiscountApplication : IColleagueDiscountApplication
    {
        private readonly IColleagueDiscountRepository _colleagueDiscountRepository;

        public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository)
        {
            _colleagueDiscountRepository = colleagueDiscountRepository;
        }

        public OperationResult Define(DefineColleagueDiscount command)
        {
            OperationResult operation = new();
            if (_colleagueDiscountRepository.Exsists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            ColleagueDiscount colleagueDiscount = new(command.ProductId, command.DiscountRate);

            _colleagueDiscountRepository.Create(colleagueDiscount);
            _colleagueDiscountRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditColleagueDiscount command)
        {
            OperationResult operation = new();
            ColleagueDiscount colleagueDiscount = _colleagueDiscountRepository.Get(command.Id);
            if (colleagueDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);


            if (_colleagueDiscountRepository.Exsists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            colleagueDiscount.Edit(command.ProductId, command.DiscountRate);

            _colleagueDiscountRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditColleagueDiscount GetDetails(long id)
        {
            return _colleagueDiscountRepository.GetDetails(id);
        }

        public OperationResult Remove(long id)
        {
            OperationResult operation = new();
            ColleagueDiscount colleagueDiscount = _colleagueDiscountRepository.Get(id);

            colleagueDiscount.Remove();

            _colleagueDiscountRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Restore(long id)
        {
            OperationResult operation = new();
            ColleagueDiscount colleagueDiscount = _colleagueDiscountRepository.Get(id);

            colleagueDiscount.Restore();

            _colleagueDiscountRepository.SaveChanges();
            return operation.Succeded();
        }

        public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
        {
            return _colleagueDiscountRepository.Search(searchModel);
        }
    }
}
