using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Comment;
using ShopManagement.Domain.CommentAgg;

namespace ShopManagement.Infrasturecure.EFCore.Repository
{
    public class CommentRepository : RepositoryBase<long, Comment>, ICommentRepository
    {
        private readonly ShopContext _context;
        public CommentRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            var query = _context.Comments
                .Include(x => x.Product)
                .Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name,
                    Message = x.Message,
                    ProductId = x.ProductId,
                    CommentDate = x.CreationDate.ToFarsi(),
                    IsCanceled = x.IsCanceled,
                    IsConfirmed = x.IsConfirmed
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name == searchModel.Name);

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
                query = query.Where(x => x.Name == searchModel.Email);

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
