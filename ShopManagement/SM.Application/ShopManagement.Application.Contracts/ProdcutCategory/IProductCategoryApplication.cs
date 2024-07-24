using _01_Framework.Application;
using System.Linq.Expressions;


namespace ShopManagement.Application.Contracts.ProdcutCategory
{
    public interface IProductCategoryApplication
    {
        OperationResult Create(CreateProductCategory command);
        OperationResult Edit(EditProductCategory command);
        EditProductCategory GetDetails(long id);
        List<ProductCategoryViewModel> GetProductCategories(bool forProductPage = false);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel serachModel);
    }
}
