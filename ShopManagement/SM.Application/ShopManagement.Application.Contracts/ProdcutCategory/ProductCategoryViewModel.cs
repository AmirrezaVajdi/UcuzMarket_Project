﻿namespace ShopManagement.Application.Contracts.ProdcutCategory
{
    public class ProductCategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string CreationDate { get; set; }
        public long ProudctsCount { get; set; }
        public string ParentName { get; set; }
    }
}
