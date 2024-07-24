using _01_Framework.Domain;
using ShopManagement.Application.Contracts.Slide;

namespace ShopManagement.Domain.SlideAgg
{
    public interface ISlideRepository : IRepository<long, Slide>
    {
        List<SlideViewModel> GetList();
        EditSlide GetDetails(long id);
    }
}
