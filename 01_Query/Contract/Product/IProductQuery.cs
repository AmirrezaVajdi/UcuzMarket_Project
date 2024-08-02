using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
using ShopManagement.Application.Contracts.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Query.Contract.Product
{
    public interface IProductQuery
    {
        ProductQueryModel GetProductDetails(string slug);
        (List<ProductQueryModel>, PaginationResult) GetPopularProducts(PaginationOptions paginationOptions);
        List<ProductQueryModel> Search(string value);
        List<CartItem> CheckInventoryStatus(List<CartItem> cartItems);
        List<ProductQueryModel> GetDiscountedProducts(int take = 4);
    }
}
