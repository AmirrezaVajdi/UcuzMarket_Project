
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
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد لطفا مجددا تلاش بفرمایید");

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
                return operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد . لطفا مجدد تلاش فرمایید");
            }

            if (_productCategoryRepository.Exsists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد لطفا مجددا تلاش بفرمایید");
            }

            var slug = command.Slug.Slugiffy();
            prodcutCategory.Edit(command.Name, command.Description, command.Picture, command.PictureAlt, command.PictureTitle, slug);
            _productCategoryRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySerachModel serachModel)
        {
            return _productCategoryRepository.Search(serachModel);
        }




    }
}
