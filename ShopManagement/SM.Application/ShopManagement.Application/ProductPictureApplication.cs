using _01_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileUploader _fileUploader;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository, IProductRepository productRepository, IFileUploader fileUploader)
        {
            _productPictureRepository = productPictureRepository;
            _productRepository = productRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            OperationResult operation = new();

            var product = _productRepository.GetProductWithCategory(command.ProductId);
            var path = $"{product.Category.Slug}//{product.Slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);

            ProductPicture productPicture = new(picturePath, command.PictureTitle, command.PicutreAlt, command.ProductId);

            _productPictureRepository.Create(productPicture);
            _productPictureRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            OperationResult operatin = new();
            var productPicture = _productPictureRepository.GetWithProductAndCategory(command.Id);
            if (productPicture == null)
                return operatin.Failed(ApplicationMessages.RecordNotFound);

            var path = $"{productPicture.Product.Category.Slug}//{productPicture.Product.Slug}";

            string picturePath = "";

            if (command.Picture is not null)
            {
                picturePath = _fileUploader.Upload(command.Picture, path);
            }
            else
            {
                picturePath = productPicture.Picture;
            }

            productPicture.Edit(picturePath, command.PictureTitle, command.PicutreAlt, command.ProductId);
            _productPictureRepository.SaveChanges();
            return operatin.Succeded();
        }

        public EditProductPicture GetDetails(long id)
        {
            return _productPictureRepository.GetDetails(id);
        }

        public OperationResult Remove(long id)
        {
            OperationResult operatin = new();
            var productPicture = _productPictureRepository.Get(id);

            if (productPicture == null)
                return operatin.Failed(ApplicationMessages.RecordNotFound);

            productPicture.Remove();
            _productPictureRepository.SaveChanges();
            return operatin.Succeded();
        }
        public OperationResult Restore(long id)
        {
            OperationResult operatin = new();
            var productPicture = _productPictureRepository.Get(id);

            if (productPicture == null)
                return operatin.Failed(ApplicationMessages.RecordNotFound);

            productPicture.Restore();
            _productPictureRepository.SaveChanges();
            return operatin.Succeded();
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            return _productPictureRepository.Search(searchModel);
        }
    }
}
