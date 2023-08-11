using _01_Framework.Application;
using _01_Query.Contract.Article;
using _01_Query.Contract.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_Query.Query
{
    public class ArticleCategoryQuery : IArticleCategoryQuery
    {
        private readonly BlogContext _context;

        public ArticleCategoryQuery(BlogContext context)
        {
            _context = context;
        }

        public List<ArticleCategoryQueryModel> GetArticleCategories()
        {
            return _context.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    ArticlesCount = x.Articles.Count,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle
                }).ToList();
        }

        public ArticleCategoryQueryModel GetArticleCategory(string slug)
        {
            var articleCategory = _context.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Slug = x.Slug,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Description = x.Description,
                    Keywords = x.Keywords,
                    Metadescription = x.Metadescription,
                    CanonicalAddress = x.CanonicalAddress,
                    Articles = MapArticles(x.Articles),
                    ArticlesCount = x.Articles.Count
                }).FirstOrDefault(x => x.Slug == slug);

            if (!string.IsNullOrWhiteSpace(articleCategory.Keywords))
                articleCategory.KeywordList = articleCategory.Keywords.Split(",").ToList();

            return articleCategory;
        }

        private static List<ArticleQueryModel> MapArticles(List<Article> articles)
        {
            return articles.Select(x => new ArticleQueryModel
            {
                Slug = x.Slug,
                ShortDescription = x.ShortDescription,
                Title = x.Title,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi()
            }).ToList();
        }
    }
}
