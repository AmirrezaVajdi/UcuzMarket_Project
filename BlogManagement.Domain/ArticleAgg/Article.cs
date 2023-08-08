using _01_Framework.Domain;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Domain.ArticleAgg
{
    public class Article : EntityBase
    {
        public string Title { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public DateTime PublishDate { get; private set; }
        public string Slug { get; private set; }
        public string MetaDescription { get; private set; }
        public string Keywords { get; private set; }
        public string CanonicalAddress { get; private set; }
        public long CategoryId { get; private set; }
        public ArticleCategory Category { get; private set; }

        public Article(string description, string title, string shortDescription, string picture, string pictureAlt, string pictureTitle, DateTime publishDate, string slug, string metaDescription, string keywords, string canonicalAddress, long categoryId)
        {
            Description = description;
            Title = title;
            ShortDescription = shortDescription;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            PublishDate = publishDate;
            Slug = slug;
            MetaDescription = metaDescription;
            Keywords = keywords;
            CanonicalAddress = canonicalAddress;
            CategoryId = categoryId;
        }

        public void Edit(string description, string title, string shortDescription, string picture, string pictureAlt, string pictureTitle, DateTime publishDate, string slug, string metaDescription, string keywords, string canonicalAddress, long categoryId)
        {
            Description = description;
            Title = title;
            ShortDescription = shortDescription;

            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            PublishDate = publishDate;
            Slug = slug;
            MetaDescription = metaDescription;
            Keywords = keywords;
            CanonicalAddress = canonicalAddress;
            CategoryId = categoryId;
        }
    }
}
