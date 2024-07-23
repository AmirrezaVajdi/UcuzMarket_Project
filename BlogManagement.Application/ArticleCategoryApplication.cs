using _01_Framework.Application;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Application
{
    public class ArticleCategoryApplication : IArticleCategoryApplication
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IFileUploader _fileUploader;
        public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
        {
            _articleCategoryRepository = articleCategoryRepository;
            this._fileUploader = fileUploader;
        }

        public OperationResult Create(CreateArticleCategory command)
        {
            OperationResult operation = new();
            if (_articleCategoryRepository.Exsists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var slug = command.Slug.Slugiffy();
            var pictureName = _fileUploader.Upload(command.Picture, slug);

            ArticleCategory articleCategory = new(command.Name, pictureName, command.PictureAlt, command.PictureTitle, command.Description, command.ShowOrder, slug, command.Keywords, command.Metadescription, command.CanonicalAddress);

            _articleCategoryRepository.Create(articleCategory);
            _articleCategoryRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditArticleCategory command)
        {
            OperationResult operation = new();
            var articleCategory = _articleCategoryRepository.Get(command.Id);

            if (articleCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_articleCategoryRepository.Exsists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var slug = command.Slug.Slugiffy();

            string pictureName = "";

            if (command.Picture is not null)
            {
                pictureName = _fileUploader.Upload(command.Picture, slug);
            }
            else
            {
                pictureName = articleCategory.Picture;
            }

            articleCategory.Edit(command.Name, pictureName, command.PictureAlt, command.PictureTitle, command.Description, command.ShowOrder, slug, command.Keywords, command.Metadescription, command.CanonicalAddress);

            _articleCategoryRepository.SaveChanges();
            return operation.Succeded();
        }

        public List<ArticleCategoryViewModel> GetArticleCategories()
        {
            return _articleCategoryRepository.GetArticleCategories();
        }

        public EditArticleCategory GetDetails(long id)
        {
            return _articleCategoryRepository.GetDetails(id);
        }

        public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
        {
            return _articleCategoryRepository.Search(searchModel);
        }
    }
}