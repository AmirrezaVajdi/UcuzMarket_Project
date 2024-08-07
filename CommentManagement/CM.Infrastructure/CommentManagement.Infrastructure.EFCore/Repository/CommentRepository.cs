using _01_Framework.Application;
using _01_Framework.Infrastructure;
using CommandManagement.Application.Contract.Comment;
using CommandManagement.Domain.CommentAgg;

namespace CommentManagement.Infrastructure.EFCore.Repository
{
    public class CommentRepository : RepositoryBase<long, Comment>, ICommentRepository
    {
        private readonly CommentContext _context;
        public CommentRepository(CommentContext context) : base(context)
        {
            _context = context;
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            var query = _context
                .Comments
                .Where(x => x.Type != CommentType.Admin)
                .Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name,
                    Message = x.Message,
                    OwnerRecordId = x.OwnerRecordId,
                    Type = x.Type,
                    CommentDate = x.CreationDate.ToFarsi(),
                    IsConfirmed = x.IsConfirmed,
                    IsCanceled = x.IsCancelled
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name == searchModel.Name);

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
                query = query.Where(x => x.Name == searchModel.Email);

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
