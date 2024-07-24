using _01_Framework.Domain;
using CommandManagement.Application.Contract.Comment;

namespace CommandManagement.Domain.CommentAgg
{
    public interface ICommentRepository : IRepository<long, Comment>
    {
        List<CommentViewModel> Search(CommentSearchModel searchModel);
    }
}
