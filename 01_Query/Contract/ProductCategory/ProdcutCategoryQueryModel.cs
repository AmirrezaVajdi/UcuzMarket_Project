﻿using _01_Query.Contract.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace _01_Query.Contract.ProductCategory
{
    public class ProdcutCategoryQueryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string KeyWords { get; set; }
        public string MetaDescription { get; set; }
        public string ProductCount { get; set; }
        public string? ParentName { get; set; }
        public string? ParentSlug { get; set; }
        public int PageNumber { get; set; }
        public long TotalPage { get; set; }
        public List<ProductQueryModel> Products { get; set; }
    }
}
