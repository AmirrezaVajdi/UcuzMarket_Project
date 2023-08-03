
using _01_Framework.Domain;
using ShopManagement.Application.Contracts.ProdcutCategory;
using System.Linq.Expressions;

namespace ShopManagement.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IRepository<long, ProductCategory>
    {
        List<ProductCategoryViewModel> GetProductCategories();
        EditProductCategory GetDetails(long id);
        string GetSlugBy(long id);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel serachModel);

    }

}