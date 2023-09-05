using ShopManagement.Application.Contracts.Order;

namespace _01_Query
{
    public interface ICartCalculatorService
    {
        Cart ComputeCart(List<CartItem> cartItems);
    }
}
