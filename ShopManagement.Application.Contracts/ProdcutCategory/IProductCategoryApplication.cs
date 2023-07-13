using _01_Framework.Application;


namespace ShopManagement.Application.Contracts.ProdcutCategory
{
    public interface IProductCategoryApplication
    {
        OperationResult Create(CreateProductCategory command);
        OperationResult Edit(EditProductCategory command);
        EditProductCategory GetDetails(long id);

        List<ProductCategoryViewModel> Search(ProductCategorySerachModel serachModel);
    }
}
