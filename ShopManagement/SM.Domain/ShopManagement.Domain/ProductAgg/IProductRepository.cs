using _01_Framework.Domain;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Domain.ProductAgg
{
    public interface IProductRepository : IRepository<long, Product>
    {
        Product GetProductWithCategory(long id);
        List<ProductViewModel> GetProducts();
        List<ProductViewModel> Search(ProductSearchModel searchModel);
        EditProduct GetDatils(long id);
    }
}
