using _01_Framework.Infrastructure;

namespace ShopManagement.Configuration.Permissions
{
    public class ShopPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new()
            {
                {
                    "محصولات",
                    new()
                    {
                        new (ShopPermissions.ListProducts, "مشاهده لیست محصولات"),
                        new (ShopPermissions.SearchProducts, "سرچ کردن در محصولات"),
                        new (ShopPermissions.CreateProduct, "ایحاد محصول"),
                        new (ShopPermissions.EditProduct, "ویرایش محصول")
                    }
                },
                {
                    "گروه محصولات",
                   new()
                    {
                        new (ShopPermissions.ListProductCategories, "مشاهده لیست گروه محصولات"),
                        new (ShopPermissions.SearchProductCategories, "سرج کردن در گروه محصولات"),
                        new (ShopPermissions.CreateProductCategory, "ایحاد گروه محصولی"),
                        new (ShopPermissions.EditProductCategory, "ویرایش گروه محصولی")
                    }
                },
                {
                    "عکس محصولات" ,
                    new()
                    {
                        new(ShopPermissions.ListProductPictures , "مشاهده لیست عکس محصولات"),
                        new(ShopPermissions.SearchProductPictures , "سرچ در عکس محصولات"),
                        new(ShopPermissions.CreateProductPicture , "ایجاد عکس محصول"),
                        new(ShopPermissions.EditProductPicture , "ویرایش عکس محصول"),
                        new(ShopPermissions.RestoreProductPicture , "بازیابی عکس محصول"),
                        new(ShopPermissions.RemoveProductPicture , "حذف عکس محصول"),
                    }
                },
                {
                    "بنر ها" ,
                    new()
                    {
                        new(ShopPermissions.ListSlides , "مشاهده لیست اسلایدر"),
                        new(ShopPermissions.CreateSlide , "ایجاد اسلایدر"),
                        new(ShopPermissions.EditSlide , "ویرایش اسلایدر"),
                        new(ShopPermissions.RemoveSlide , "حذف کردن اسلایدر"),

                    }
                },
                {
                    "سفارشات" ,
                    new()
                    {
                        new(ShopPermissions.ListOrders , "لیست سفارشات"),
                        new(ShopPermissions.SearchOrders , "سرچ کردن در سفارشات"),
                        new(ShopPermissions.ConfirmOrder , "تایید سفارش"),
                        new(ShopPermissions.CancelOrder , "لغو سفارش"),
                        new(ShopPermissions.ItemsOrder , "آیتم ها"),
                    }
                }
            };
        }
    }
}
