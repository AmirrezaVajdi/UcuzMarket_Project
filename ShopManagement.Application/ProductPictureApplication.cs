using _01_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductPictureRepository _productPictureRepository;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository)
        {
            _productPictureRepository = productPictureRepository;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            OperationResult operation = new();
            if (_productPictureRepository.Exsists(x => x.Picture == command.Picture && x.ProductId == command.ProductId))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            ProductPicture productPicture = new(command.Picture, command.PictureTitle, command.PicutreAlt, command.ProductId);

            _productPictureRepository.Create(productPicture);
            _productPictureRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            OperationResult operatin = new();
            var productPicture = _productPictureRepository.Get(command.Id);
            if (productPicture == null)
                return operatin.Failed(ApplicationMessages.RecordNotFound);

            if (_productPictureRepository.Exsists(x => x.Picture == command.Picture && x.ProductId == command.ProductId && x.Id == command.Id))
                return operatin.Failed(ApplicationMessages.DuplicatedRecored);

            productPicture.Edit(command.Picture, command.PictureTitle, command.PicutreAlt, command.ProductId);
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
