using ShopManagement.Application.Contracts.Product;

namespace DiscountManagment.Application.Contract.ColleagueDiscount
{
    public class DefineColleagueDiscount
    {
        public long ProductId { get; set; }
        public int DiscountRate { get; set; }
        List<ProductViewModel> Products { get; set; }
    }
}
