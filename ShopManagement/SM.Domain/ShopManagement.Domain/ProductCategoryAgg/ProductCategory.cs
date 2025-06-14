﻿using _01_Framework.Domain;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Domain.ProductCategoryAgg
{
    public class ProductCategory : EntityBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public string KeyWords { get; private set; }
        public string MetaDescription { get; private set; }
        public string Slug { get; private set; }
        public ProductCategory Parent { get; set; }
        public long? ParentId { get; set; }
        public List<ProductCategory> Children { get; set; }
        public List<Product> Products { get; private set; }

        public ProductCategory(string name, string description, string picture, string pictureAlt, string pictureTitle, string keyWords, string metaDescription, string slug, long? parentId)
        {
            Name = name;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            KeyWords = keyWords;
            MetaDescription = metaDescription;
            Slug = slug;
            Products = new();
            ParentId = parentId;
        }

        public void Edit(string name, string description, string picture, string pictureAlt, string pictureTitle, string slug, long? parentId)
        {
            Name = name;
            Description = description;
            if (!string.IsNullOrWhiteSpace(Picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Slug = slug;
            ParentId = parentId;

        }
    }
}
