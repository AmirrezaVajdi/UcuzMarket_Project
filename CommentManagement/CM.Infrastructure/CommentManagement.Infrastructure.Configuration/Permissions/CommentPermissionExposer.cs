using _01_Framework.Infrastructure;

namespace CommentManagement.Infrastructure.Configuration.Permissions
{
    public class CommentPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new()
          {
              {
                  "کامنت ها" ,
                  new()
                  {
                      new(CommentPermission.ListComments , "مشاهده کامنت ها"),
                      new(CommentPermission.SearchComment , "سرج کردن در کامنت ها"),
                      new(CommentPermission.ConfirmComment , "تایی کردن کامنت"),
                      new(CommentPermission.CancelComment , "کنسل کردن کامنت"),
                  }
              }
          };
        }
    }
}
