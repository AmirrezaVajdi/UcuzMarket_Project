﻿using _01_Framework.Domain;

namespace ShopManagement.Domain.OrderAgg
{
    public class Order : EntityBase
    {
        public long AccountId { get; private set; }
        public int PaymentMethod { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public double PayAmount { get; private set; }
        public bool IsPaid { get; private set; }
        public bool IsCanceled { get; private set; }
        public string IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }
        public string Address { get; private set; }
        public List<OrderItem> Items { get; private set; }

        public Order(long accountId, int paymentMethod, double totalAmount, double discountAmount, double payAmount, string address)
        {
            AccountId = accountId;
            PaymentMethod = paymentMethod;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
            IsPaid = false;
            IsCanceled = false;
            RefId = 0;
            Items = new();
            Address = address;
        }

        public void PaymentSucceeded(long refId)
        {
            IsPaid = true;
            RefId = refId != 0 ? refId : default;
        }

        public void SetIssueTrackingNo(string number)
        {
            IssueTrackingNo = number;
        }

        public void Cancel()
        {
            IsCanceled = true;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }
    }

}
