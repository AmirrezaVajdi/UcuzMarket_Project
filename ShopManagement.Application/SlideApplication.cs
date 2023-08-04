using _01_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application
{
    public class SlideApplication : ISlideApplication
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IFileUploader _fileUploader;
        public SlideApplication(ISlideRepository slideRepository, IFileUploader fileUploader)
        {
            _slideRepository = slideRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateSlide command)
        {
            OperationResult operation = new();

            var pictureName = _fileUploader.Upload(command.Picture, "Slides");

            var slide = new Slide(pictureName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.Link, command.BtnText);
            _slideRepository.Create(slide);
            _slideRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Remove(long id)
        {
            OperationResult operation = new();
            var slide = _slideRepository.Get(id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Remove();

            _slideRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditSlide command)
        {
            OperationResult operation = new();
            var slide = _slideRepository.Get(command.Id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var pictureName = _fileUploader.Upload(command.Picture, "Slides");


            slide.Edit(pictureName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.Link, command.BtnText);

            _slideRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditSlide GetDetails(long id)
        {
            return _slideRepository.GetDetails(id);
        }

        public List<SlideViewModel> GetList()
        {
            return _slideRepository.GetList();
        }

        public OperationResult Restore(long id)
        {
            OperationResult operation = new();
            var slide = _slideRepository.Get(id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Restore();

            _slideRepository.SaveChanges();
            return operation.Succeded();
        }
    }
}
