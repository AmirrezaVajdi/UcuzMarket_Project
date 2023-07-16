using _01_Framework.Domain;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Domain.ProductPictureAgg
{
    public class ProductPicture : EntityBase
    {
        public string Picture { get; private set; }
        public string PictureTitle { get; private set; }
        public string PicutreAlt { get; private set; }
        public bool IsRemoved { get; private set; }
        public Product Product { get; private set; }
        public long ProductId { get; private set; }


        public ProductPicture(string picture, string pictureTitle, string picutreAlt, long productId)
        {
            Picture = picture;
            PictureTitle = pictureTitle;
            PicutreAlt = picutreAlt;
            ProductId = productId;
            IsRemoved = false;
        }

        public void Edit(string picture, string pictureTitle, string picutreAlt, long productId)
        {
            Picture = picture;
            PictureTitle = pictureTitle;
            PicutreAlt = picutreAlt;
            ProductId = productId;
        }

        public void Remove()
        {
            IsRemoved = true;
        }
        public void Restore()
        {
            IsRemoved = false;
        }

    }
}
