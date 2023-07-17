using _01_Query.Contract.Slide;
using ShopManagement.Infrasturecure.EFCore;

namespace _01_Query.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopContext _shopContext;

        public SlideQuery(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public List<SldieQueryModel> GetSlides()
        {
            return _shopContext.Slides.Where(x => x.IsRemoved == false).Select(x => new SldieQueryModel
            {
                Text = x.Text,
                Heading = x.Heading,
                Title = x.Title,
                BtnText = x.BtnText,
                Link = x.Link,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle
            }).ToList();
        }
    }
}
