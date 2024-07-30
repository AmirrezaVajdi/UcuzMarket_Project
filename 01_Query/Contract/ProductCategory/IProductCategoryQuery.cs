using _01_Framework.Application.Pagination;
using _01_Query.Contract.Product;
using _01_Query.Query;

namespace _01_Query.Contract.ProductCategory
{
    public interface IProductCategoryQuery
    {
        ProdcutCategoryQueryModel GetProductCategoryWithProductsBy(string slug, PaginationOptions paginationOptions, FilteringOptions<ProdcutCategoryQueryModel> filteringOptions, SortingOptions<ProdcutCategoryQueryModel> sortingOptions);
        List<ProdcutCategoryQueryModel> GetProductCategories();
        List<ProdcutCategoryQueryModel> GetProductCategoriesWithProducts();
        List<ProductCategoryWithChildren> GetCategoryWithChildren();
        List<ProductCategoryWithChildren> GetCategoryAndPictureWithChildren();
        List<ProdcutCategoryQueryModel> GetCategryWithProudctCount();
    }
}
