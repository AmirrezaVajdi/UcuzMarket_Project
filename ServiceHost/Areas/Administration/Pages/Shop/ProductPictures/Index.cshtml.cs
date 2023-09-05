using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductPictures
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductPictureSearchModel SearchModel { get; set; }
        public List<ProductPictureViewModel> ProductPictures { get; set; }
        public SelectList Product { get; set; }
        private IProductApplication _productApplication;
        private readonly IProductPictureApplication _productPictureApplication;

        public IndexModel(IProductApplication productApplication, IProductPictureApplication productPictureApplication)
        {
            _productApplication = productApplication;
            _productPictureApplication = productPictureApplication;
        }

        [NeedPermission(ShopPermissions.ListProductPictures)]
        public void OnGet(ProductPictureSearchModel searchModel)
        {
            Product = new(_productApplication.GetProducts(), "Id", "Name");
            ProductPictures = _productPictureApplication.Search(searchModel);
        }

        [NeedPermission(ShopPermissions.CreateProductPicture)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateProductPicture
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        [NeedPermission(ShopPermissions.CreateProductPicture)]
        public JsonResult OnPostCreate(CreateProductPicture command)
        {
            var result = _productPictureApplication.Create(command);
            return new JsonResult(result);
        }

        [NeedPermission(ShopPermissions.EditProductPicture)]
        public IActionResult OnGetEdit(long id)
        {
            var productPicture = _productPictureApplication.GetDetails(id);
            productPicture.Products = _productApplication.GetProducts();
            return Partial("./Edit", productPicture);
        }

        [NeedPermission(ShopPermissions.EditProductPicture)]
        public JsonResult OnPostEdit(EditProductPicture command)
        {
            var result = _productPictureApplication.Edit(command);
            return new JsonResult(result);
        }

        [NeedPermission(ShopPermissions.RemoveProductPicture)]
        public IActionResult OnGetRemove(long id)
        {
            var result = _productPictureApplication.Remove(id);
            if (result.isSuccedded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedPermission(ShopPermissions.RestoreProductPicture)]
        public IActionResult OnGetRestore(long Id)
        {
            var result = _productPictureApplication.Restore(Id);
            if (result.isSuccedded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
