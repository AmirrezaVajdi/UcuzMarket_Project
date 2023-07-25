using _01_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;

        public ProductApplication(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public OperationResult Create(CreateProduct command)
        {
            OperationResult operation = new();
            if (_productRepository.Exsists(x => x.Name == command.Name))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecored);
            }

            var slug = command.Slug.Slugiffy();

            Product product = new(command.Name, command.Code ,command.ShortDescription, command.Description, command.Picture, command.PictureAlt, command.PictureTitle, slug, command.KeyWords, command.MetaDescription, command.CategoryId);
            _productRepository.Create(product);
            _productRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditProduct command)
        {
            OperationResult operation = new();
            var product = _productRepository.Get(command.Id);
            if (_productRepository.Exsists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecored);
            }
            if (product == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            var slug = command.Slug.Slugiffy();

            product.Edit(command.Name, command.Code, command.ShortDescription, command.Description, command.Picture, command.PictureAlt, command.PictureTitle, slug, command.KeyWords, command.MetaDescription, command.CategoryId);
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
