
using _01_Framework.Application;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository repository)
        {
            _productCategoryRepository = repository;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            OperationResult operation = new();
            if (_productCategoryRepository.Exsists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var slug = command.Slug.Slugiffy();
            var productCategory = new ProductCategory(command.Name, command.Description, command.Picture, command.PictureAlt
                , command.PictureTitle, command.KeyWords, command.MetaDescription, slug);

            _productCategoryRepository.Create(productCategory);
            _productCategoryRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            OperationResult operation = new();
            var prodcutCategory = _productCategoryRepository.Get(command.Id);
            if (prodcutCategory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            if (_productCategoryRepository.Exsists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecored);
            }

            var slug = command.Slug.Slugiffy();
            prodcutCategory.Edit(command.Name, command.Description, command.Picture, command.PictureAlt, command.PictureTitle, slug);
            _productCategoryRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditProductCategory GetDatails(long id)
        {
            throw new NotImplementedException();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            return _productCategoryRepository.GetProductCategories();
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel serachModel)
        {
            return _productCategoryRepository.Search(serachModel);
        }




    }
}
