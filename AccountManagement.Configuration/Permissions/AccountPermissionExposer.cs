using _01_Framework.Infrastructure;

namespace AccountManagement.Configuration.Permissions
{
    public class AccountPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
          {
              {
                  "کاربران" ,
                  new()
                  {
                      new(AccountPermission.RegisterAccount , "ثبت نام کاربر"),
                      new(AccountPermission.EditAccount , "ویرایش کاربر"),
                      new(AccountPermission.ChangePassword , "تغیر رمز کاربر"),
                      new(AccountPermission.ListAccount , "لیست کاربران"),
                  }
                },
                {
                "نقش ها" ,
                    new()
                    {
                        new(AccountPermission.ListRole , "لیست نقش ها") ,
                        new(AccountPermission.CreateRole , "ایجاد نقش"),
                        new(AccountPermission.EditRole , "ویرایش نقش")
                    }
                }
          };
        }
    }
}
