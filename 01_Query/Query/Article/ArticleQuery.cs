using _01_Framework.Application;
using _01_Query.Contract.Article;
using _01_Query.Contract.Comment;
using _01_Query.Contract.Product;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_Query.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _context;
        private readonly CommentContext _commentContext;

        public ArticleQuery(BlogContext context, CommentContext commentContext)
        {
            _context = context;
            _commentContext = commentContext;
        }

        public ArticleQueryModel GetArticleDetails(string slug)
        {
            var article = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate <= DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    Slug = x.Slug,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    CategoryName = x.Category.Name,
                    PublishDate = x.PublishDate.ToFarsi(),
                    CategorySlug = x.Category.Slug,
                    MetaDescription = x.MetaDescription,
                    CanonicalAddress = x.CanonicalAddress,
                    Keywords = x.Keywords
                }).FirstOrDefault(x => x.Slug == slug);

            article.KeywordList = article.Keywords.Split(",").ToList();

            var comments = _commentContext.Comments
                .Where(x => x.Type == CommentType.Article)
                .Where(x => x.OwnerRecordId == article.Id)
                .Where(x => x.IsConfirmed)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Message = x.Message,
                    CreationDate = x.CreationDate.ToFarsi(),
                    ChildId = x.ChildId,
                })
                .OrderByDescending(x => x.Id)
                .ToList();

            foreach (var comment in comments)
            {
                if (comment.ChildId > 0)
                    comment.ParentName = comments.FirstOrDefault(x => x.Id == comment.ChildId)?.Name;
            }

            article.Comments = comments;

            return article;
        }

        public List<ArticleQueryModel> LatestArticles(int take = 6)
        {
            return _context
                .Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate <= DateTime.Now)
                .OrderByDescending(x => x.Id)
                .Select(x => new ArticleQueryModel
                {
                    Title = x.Title,
                    Slug = x.Slug,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    PublishDate = x.PublishDate.ToFarsi(),
                    ShortDescription = x.ShortDescription
                })
                .Take(take)
                .ToList();
        }
    }
}
