using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Infrastructure.EFCore;

namespace ShopManagement.Infrasturecure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
    {
        private readonly ShopContext _context;
        private readonly AccountContext _accountContext;

        public OrderRepository(ShopContext context, AccountContext accountContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
        }

        public double GetAmountBy(long id)
        {
            var order = _context.Orders.Select(x => new { x.Id, x.PayAmount }).FirstOrDefault(x => x.Id == id);

            if (order != null)
                return order.PayAmount;
            return 0;
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            var products = _context.Products.Select(x => new { x.Id, x.Name }).ToList();
            var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null) return new();
            var items = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                Count = x.Count,
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId,
                OrderId = x.OrderId,
                UnitPrice = x.UnitPrice,
            }).ToList();

            items.ForEach(x => x.Product = products.FirstOrDefault(z => z.Id == x.ProductId)?.Name);

            return items;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            var accounts = _accountContext.Accounts.Select(x => new { x.Id, x.Fullname }).ToList();
            var query = _context.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                AccountId = x.AccountId,
                DiscountAmount = x.DiscountAmount,
                CreationDate = x.CreationDate.ToFarsi(),
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount
            });

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0)
            {
                query = query.Where(x => x.AccountId == searchModel.AccountId);
            }

            var orders = query.OrderByDescending(x => x.Id).ToList();

            foreach (var order in orders)
            {
                order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.Fullname;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
            }

            return orders;
        }
    }
}
