using _01_Framework.Infrastructure;

namespace BlogManagement.Infrastructure.Configuration.Permissions
{
    public class BlogPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new()
            {
                {
                    "گروه مقالات" ,
                    new()
                    {
                        new(BlogPermission.ListArticleCategories , "مشاهده گروه مقالات"),
                        new(BlogPermission.SearchArticleCategories , "سرج کردن در گروه مقالات"),
                        new(BlogPermission.CreateArticleCategory , "ایجاد گروه مقالات"),
                        new(BlogPermission.EditArticleCategory , "ویرایش گروه مقالات"),
                    }
                } ,
                {
                    "مقالات" ,
                    new()
                    {
                        new(BlogPermission.ListArticles , "مشاهده مقالات"),
                        new(BlogPermission.SearchArticles , "سرچ کردن در مقالات"),
                        new(BlogPermission.CreateArticle , "ایجاد مقاله"),
                        new(BlogPermission.EditArticle , "ویرایش مقاله"),
                    }
                }
            };
        }
    }
}
