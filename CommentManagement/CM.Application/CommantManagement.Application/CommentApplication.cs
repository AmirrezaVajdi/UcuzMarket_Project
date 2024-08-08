using _01_Framework.Application;
using CommandManagement.Application.Contract.Comment;
using CommandManagement.Domain.CommentAgg;
using CommantManagement.Application.Contract.Comment;
using ShopManagement.Application.Contracts.Comment;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CommandManagement.Application
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
            var comment = new Comment(command.Name, command.Email, command.Message, command.OwnerRecordId, command.Type, command.ChildId);

            _repository.Create(comment);
            _repository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult AddAdminReply(AdminReplyComment command)
        {
            OperationResult operation = new();

            try
            {
                var comment = _repository.Get(command.ParentId);

                comment.AddAdminReply(new Comment("ادمین اوجوز مارکت", "admin@ucuzmarket.ir", command.Message, comment.OwnerRecordId, 0, null));

                _repository.SaveChanges();

                return operation.Succeded();
            }
            catch
            {
                return operation.Failed();
            }
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

        public string GetCommentNameBy(long id)
        {
            return _repository.Get(id).Name;
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            return _repository.Search(searchModel);
        }
    }
}
