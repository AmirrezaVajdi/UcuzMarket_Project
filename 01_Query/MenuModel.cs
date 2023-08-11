using _01_Query.Contract.ArticleCategory;
using _01_Query.Contract.ProductCategory;
using _01_Query.Query;

namespace _01_Query
{
    public class MenuModel
    {
        public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }
        public List<ProdcutCategoryQueryModel> ProductCategories { get; set; }
    }
}
