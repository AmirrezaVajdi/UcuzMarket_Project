using _01_Framework.Application;
using CommandManagement.Application.Contract.Comment;
using CommantManagement.Application.Contract.Comment;

namespace ShopManagement.Application.Contracts.Comment
{
    public interface ICommentApplication
    {
        OperationResult Add(AddComment command);
        OperationResult AddAdminReply(AdminReplyComment command);
        OperationResult Confirm(long id);
        OperationResult Cancel(long id);
        List<CommentViewModel> Search(CommentSearchModel searchModel);
        string GetCommentNameBy(long id);
    }
}
