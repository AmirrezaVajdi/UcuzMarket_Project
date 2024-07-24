namespace ShopManagement.Application.Contracts.Order
{
    public class Cart
    {
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double PayAmmount { get; set; }
        public int PaymentMethod { get; set; }
        public List<CartItem> Items { get; set; }

        public Cart()
        {
            Items = new();
        }

        public void Add(CartItem cartItem)
        {
            Items.Add(cartItem);
            TotalAmount += cartItem.TotalItemPrice;
            DiscountAmount += cartItem.DiscountAmount;
            PayAmmount += cartItem.ItemPayAmount;
        }

        public void SetPaymentMethod(int methodId)
        {
            PaymentMethod = methodId;
        }
    }
}
