using _01_Query.Contract.Article;
using _01_Query.Contract.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleQueryModel Article;
        public List<ArticleQueryModel> LatestArticles;
        public List<ArticleCategoryQueryModel> ArticleCategories;

        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        public ArticleModel(IArticleQuery articleQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _articleQuery = articleQuery;
            LatestArticles = _articleQuery.LatestArticles();
            _articleCategoryQuery = articleCategoryQuery;
        }

        public void OnGet(string id)
        {
            Article = _articleQuery.GetArticleDetails(id);
            ArticleCategories = _articleCategoryQuery.GetArticleCategories();
        }
    }
}
