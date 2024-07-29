using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductCategoryRepository : RepositoryBase<long, ProductCategory>, IProductCategoryRepository
    {
        private readonly ShopContext _context;

        public ProductCategoryRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public EditProductCategory GetDetails(long id)
        {
            return _context.ProductCategories.Select(x => new EditProductCategory
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                //Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                KeyWords = x.KeyWords,
                MetaDescription = x.MetaDescription,
                Slug = x.Slug,
                ParentId = x.ParentId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ProductCategoryViewModel> GetProductCategories(bool forProductPage = false)
        {
            List<ProductCategoryViewModel> result = new();
            if (!forProductPage)
            {
                result = _context.ProductCategories
                   .Where(x => x.ParentId == null)
                   .Select(x => new ProductCategoryViewModel
                   {
                       Id = x.Id,
                       Name = x.Name
                   })
                   .ToList();
            }
            else
            {
                result = _context.ProductCategories
                    .Where(x => x.ParentId != null)
                   .Select(x => new ProductCategoryViewModel
                   {
                       Id = x.Id,
                       Name = x.Name
                   })
                   .ToList();
            }
            return result;
        }

        public string GetSlugBy(long id)
        {
            return _context.ProductCategories.Select(x => new { x.Id, x.Slug }).FirstOrDefault(x => x.Id == id).Slug;
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel serachModel)
        {
            var query = _context.ProductCategories.ToList();

            if (!string.IsNullOrWhiteSpace(serachModel.Name))
            {
                query = query.Where(x => x.Name.Contains(serachModel.Name)).ToList();
            }

            List<ProductCategoryViewModel> vm = new(query.Count);

            query.ForEach(x => vm.Add(new()
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                Picture = x.Picture,
                ParentName = (x.Parent == null ? "دسته بندی اصلی" : x.Parent.Name)
            }));

            return vm;
        }
    }
}
