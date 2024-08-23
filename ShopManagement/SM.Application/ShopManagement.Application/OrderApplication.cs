using _0_Framework.Application;
using _0_Framework.Application.Sms;
using _01_Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly IShopInventoryAcl _shopInventoryAcl;
        private readonly ISmsService _smsService;
        private readonly IShopAccountAcl _shopAccountAcl;

        public OrderApplication(IAuthHelper authHelper, IOrderRepository orderRepository, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl, ISmsService smsService, IShopAccountAcl shopAccountAcl)
        {
            _authHelper = authHelper;
            _orderRepository = orderRepository;
            _configuration = configuration;
            _shopInventoryAcl = shopInventoryAcl;
            _smsService = smsService;
            _shopAccountAcl = shopAccountAcl;
        }

        public void Cancel(long Id)
        {
            var order = _orderRepository.Get(Id);
            order.Cancel();
            _orderRepository.SaveChanges();
        }

        public double GetAmountBy(long id)
        {
            return _orderRepository.GetAmountBy(id);
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            return _orderRepository.GetItems(orderId);
        }

        public string PaymentSucceeded(long orderId, long refId)
        {
            var order = _orderRepository.Get(orderId);
            order.PaymentSucceeded(refId);
            var symbol = _configuration.GetSection("symbol").Value;
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            order.SetIssueTrackingNo(issueTrackingNo);
            if (_shopInventoryAcl.ReduceFromInventory(order.Items))
            {
                _orderRepository.SaveChanges();
                //var account = _shopAccountAcl.GetAccountBy(order.AccountId);
                //_smsService.Send(account.mobile, $"{account.name} گرامی پرداخت شمما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و به زودی برای شما ارسال خواهد شد");
                return issueTrackingNo;
            }
            return "";
        }

        public long PlaceOrder(Cart cart, string address)
        {
            var currentAccountId = _authHelper.CurrentAccountId();

            Order order = new(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount, cart.PayAmmount, address);

            foreach (var item in cart.Items)
            {
                OrderItem orderItem = new(item.Id, item.Count, item.UnitPrice, item.DiscountRate);
                order.Items.Add(orderItem);
            }
            _orderRepository.Create(order);
            _orderRepository.SaveChanges();
            return order.Id;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return _orderRepository.Search(searchModel);
        }
    }
}
