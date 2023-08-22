using _01_Framework.Infrastructure;

namespace DiscountManagement.Configuration.Permissions
{
    public class DiscountPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new()
           {
               {
                   "تخفیف مشتری" ,
                   new()
                   {
                       new(DiscountPermission.ListCustomerDiscounts , "مشاهده لیست تخفیفات مشتری"),
                       new(DiscountPermission.SearchCustomerDiscounts , "سرج کردن در تخفیفات مشتری"),
                       new(DiscountPermission.DefineCustomerDiscount , "تعریف تخفیف مشتری"),
                       new(DiscountPermission.EditCustomerDiscount , "ویرایش تخفیف مشتری")
                   }
               },
                 {
                   "تخفیف همکاری" ,
                   new()
                   {
                        new(DiscountPermission.ListColleagueDiscounts , "مشاهده لیست تخفیفات همکاری"),
                       new(DiscountPermission.SearchColleagueDiscounts , "سرج کردن در تخفیفات "),
                       new(DiscountPermission.DefineColleagueDiscount , "تعریف تخفیف همکاری"),
                       new(DiscountPermission.EditColleagueDiscount , "ویرایش تخفیف همکاری"),
                       new(DiscountPermission.RestoreColleagueDiscount , "بازیابی تخفیف همکاری"),
                       new(DiscountPermission.RemoveColleagueDiscount , "حذف کردن تخفیف همکاری")

                   }
               },
           };
        }
    }
}
