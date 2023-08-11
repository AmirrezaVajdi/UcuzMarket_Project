using _01_Framework.Application;
using _01_Query.Contract.Article;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_Query.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _context;

        public ArticleQuery(BlogContext context)
        {
            _context = context;
        }

        public ArticleQueryModel GetArticleDetails(string slug)
        {
            var article = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate <= DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
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

            return article;
        }

        public List<ArticleQueryModel> LatestArticles()
        {
            return _context.Articles.Include(x => x.Category).Where(x => x.PublishDate <= DateTime.Now).Select(x => new ArticleQueryModel
            {
                Title = x.Title,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription
            }).ToList();
        }
    }
}
