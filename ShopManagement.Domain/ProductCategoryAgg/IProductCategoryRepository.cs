
using _01_Framework.Domain;
using ShopManagement.Application.Contracts.ProdcutCategory;
using System.Linq.Expressions;

namespace ShopManagement.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IRepository<long , ProductCategory>
    {
        EditProductCategory GetDetails(long id);
        List<ProductCategoryViewModel> Search(ProductCategorySerachModel serachModel);

    }

}