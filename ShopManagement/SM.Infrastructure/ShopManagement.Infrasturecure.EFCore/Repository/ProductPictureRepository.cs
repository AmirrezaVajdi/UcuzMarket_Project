﻿using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductPictureRepository : RepositoryBase<long, ProductPicture>, IProductPictureRepository
    {
        private readonly ShopContext _context;

        public ProductPictureRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public EditProductPicture GetDetails(long id)
        {
            return _context.ProductPictures.Select(x => new EditProductPicture
            {
                Id = x.Id,
                PictureTitle = x.PictureTitle,
                PicutreAlt = x.PicutreAlt,
                ProductId = x.ProductId
            }).FirstOrDefault(x => x.Id == id);
        }

        public ProductPicture GetWithProductAndCategory(long id)
        {
            return _context.ProductPictures.Include(x => x.Product).ThenInclude(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            var query = _context.ProductPictures.Include(x => x.Product).Select(x => new ProductPictureViewModel
            {
                Id = x.Id,
                Picture = x.Picture,
                Product = x.Product.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                ProductId = x.ProductId,
                IsRemoved = x.IsRemoved
            });

            if (searchModel.ProductId != 0)
            {
                query = query.Where(x => x.ProductId == searchModel.ProductId);
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
