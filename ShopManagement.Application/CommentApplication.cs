using _01_Framework.Application;
using ShopManagement.Application.Contracts.Comment;
using ShopManagement.Domain.CommentAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class CommentApplication : ICommentApplication
    {
        private readonly ICommentRepository _repository;

        public CommentApplication(ICommentRepository repository)
        {
            _repository = repository;
        }

        public OperationResult Add(AddComment command)
        {
            OperationResult operation = new();
            var comment = new Comment(command.Name, command.Email, command.Message, command.ProductId);
            _repository.Create(comment);
            _repository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Cancel(long id)
        {
            OperationResult operation = new();
            var comment = _repository.Get(id);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            comment.Cancel();
            _repository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Confirm(long id)
        {
            OperationResult operation = new();
            var comment = _repository.Get(id);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            comment.Confirm();
            _repository.SaveChanges();
            return operation.Succeded();
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            return _repository.Search(searchModel);
        }
    }
}
