﻿using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Domain.Services
{
    public interface IShopInventoryAcl
    {
        bool ReduceFromInventory(List<OrderItem> items);
    }
}
