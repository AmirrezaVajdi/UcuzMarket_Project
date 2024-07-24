using _01_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploader _fileUploader;
        private readonly IProductCategoryRepository _productCategoryRepository;
        public ProductApplication(IProductRepository productRepository, IFileUploader fileUploader, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _fileUploader = fileUploader;
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProduct command)
        {
            OperationResult operation = new();
            if (_productRepository.Exsists(x => x.Name == command.Name))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecored);
            }

            var slug = command.Slug.Slugiffy();
            var categorySlug = _productCategoryRepository.GetSlugBy(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);

            Product product = new(command.Name, command.Code, command.ShortDescription, command.Description, picturePath, command.PictureAlt, command.PictureTitle, slug, command.KeyWords, command.MetaDescription, command.CategoryId);
            _productRepository.Create(product);
            _productRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditProduct command)
        {
            OperationResult operation = new();
            var product = _productRepository.GetProductWithCategory(command.Id);
            if (_productRepository.Exsists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecored);
            }
            if (product == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            var slug = command.Slug.Slugiffy();
            var path = $"{product.Category.Slug}/{slug}";

            string picturePath = "";

            if (command.Picture is not null)
            {
                picturePath = _fileUploader.Upload(command.Picture, path);
            }
            else
            {
                picturePath = product.Picture;
            }

            product.Edit(command.Name, command.Code, command.ShortDescription, command.Description, picturePath, command.PictureAlt, command.PictureTitle, slug, command.KeyWords, command.MetaDescription, command.CategoryId);
            _productRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditProduct GetDetails(long id)
        {
            return _productRepository.GetDatils(id);
        }

        public List<ProductViewModel> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            return _productRepository.Search(searchModel);
        }
    }
}
