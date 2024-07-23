using _01_Query;
using _01_Query.Contract.ArticleCategory;
using _01_Query.Contract.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _ProductCategoryQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        public MenuViewComponent(IProductCategoryQuery productCategoryQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _ProductCategoryQuery = productCategoryQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            var result = new MenuModel()
            {
                ArticleCategories = _articleCategoryQuery.GetArticleCategories(),
                ProductCategories = _ProductCategoryQuery.GetCategoryWithChildren()
            };

            return View(result);
        }
    }
}
