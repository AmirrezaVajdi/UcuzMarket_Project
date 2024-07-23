using _01_Query.Contract.Product;
using _01_Query.Query;

namespace _01_Query.Contract.ProductCategory
{
    public interface IProductCategoryQuery
    {
        ProdcutCategoryQueryModel GetProductCategoryWithProductsBy(string slug);
        List<ProdcutCategoryQueryModel> GetProductCategories();
        List<ProdcutCategoryQueryModel> GetProductCategoriesWithProducts();
        List<ProductCategoryWithChildren> GetCategoryWithChildren();
    }
}
