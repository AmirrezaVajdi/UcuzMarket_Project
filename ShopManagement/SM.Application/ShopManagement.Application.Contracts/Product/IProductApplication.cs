﻿using _01_Framework.Application;

namespace ShopManagement.Application.Contracts.Product
{
    public interface IProductApplication
    {
        OperationResult Create(CreateProduct command);
        OperationResult Edit(EditProduct command);
        List<ProductViewModel> GetProducts();
        EditProduct GetDetails(long id);
        List<ProductViewModel> Search(ProductSearchModel searchModel);

    }
}
