using _01_Framework.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Application
{
    public class ArticleApplication : IArticleApplication
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IFileUploader _fileUploader;

        public ArticleApplication(IArticleRepository articleRepository, IFileUploader fileUploader, IArticleCategoryRepository articleCategoryRepository)
        {
            _articleRepository = articleRepository;
            _fileUploader = fileUploader;
            _articleCategoryRepository = articleCategoryRepository;
        }

        public OperationResult Create(CreateArticle command)
        {
            OperationResult operation = new();
            if (_articleRepository.Exsists(x => x.Title == command.Title))
                operation.Failed(ApplicationMessages.DuplicatedRecored);

            var slug = command.Slug.Slugiffy();
            var categorySlug = _articleCategoryRepository.GetSlugBy(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var pictureName = _fileUploader.Upload(command.Picture, path);
            var publishDate = command.PublishDate.ToGeorgianDateTime();

            Article article = new(command.Description, command.Title, command.ShortDescription, pictureName, command.PictureAlt, command.PictureTitle, publishDate, slug, command.MetaDescription, command.Keywords, command.CanonicalAddress, command.CategoryId);

            _articleRepository.Create(article);
            _articleRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditArticle command)
        {
            OperationResult operation = new();
            var article = _articleRepository.GetWithCategory(command.Id);
            if (article == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_articleRepository.Exsists(x => x.Title == command.Title && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var slug = command.Slug.Slugiffy();
            var path = $"{article.Category.Slug}/{slug}";

            string pictureName = "";

            if (command.Picture is not null)
            {
                pictureName = _fileUploader.Upload(command.Picture, path);
            }
            else
            {
                pictureName = article.Picture;
            }

            var publishDate = command.PublishDate.ToGeorgianDateTime();

            article.Edit(command.Description, command.Title, command.ShortDescription, pictureName, command.PictureAlt, command.PictureTitle, publishDate, slug, command.MetaDescription, command.Keywords, command.CanonicalAddress, command.CategoryId);

            _articleRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditArticle GetDetails(long id)
        {
            return _articleRepository.GetDetails(id);
        }

        public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
        {
            return _articleRepository.Search(searchModel);
        }
    }
}