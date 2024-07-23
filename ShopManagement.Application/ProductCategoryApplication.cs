
using _01_Framework.Application;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IFileUploader _fileUploader;

        public ProductCategoryApplication(IProductCategoryRepository repository, IFileUploader fileUploader)
        {
            _productCategoryRepository = repository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            OperationResult operation = new();
            if (_productCategoryRepository.Exsists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var slug = command.Slug.Slugiffy();
            var picturepath = $"{command.Slug}";
            var fileName = _fileUploader.Upload(command.Picture, picturepath);
            var productCategory = new ProductCategory(command.Name, command.Description, fileName, command.PictureAlt
                , command.PictureTitle, command.KeyWords, command.MetaDescription, slug, command.ParentId);

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
            var picturepath = $"{command.Slug}";
            string fileName = "";
            if (command.Picture is not null)
            {
                fileName = _fileUploader.Upload(command.Picture, picturepath);
            }
            else
            {
                fileName = prodcutCategory.Picture;
            }

            prodcutCategory.Edit(command.Name, command.Description, fileName, command.PictureAlt, command.PictureTitle, slug, command.ParentId);
            _productCategoryRepository.SaveChanges();
            return operation.Succeded();
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
