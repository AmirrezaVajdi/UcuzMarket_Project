namespace ShopManagement.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        double GetAmountBy(long id);
        long PlaceOrder(Cart cart , string address);
        string PaymentSucceeded(long orderId, long refId);
        List<OrderViewModel> Search(OrderSearchModel searchModel);
        List<OrderItemViewModel> GetItems(long orderId);
        void Cancel(long Id);
    }
}
