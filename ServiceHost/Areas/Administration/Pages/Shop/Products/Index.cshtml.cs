using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Infrastructure.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.Product
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductSearchModel SearchModel { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public SelectList ProductCategories { get; set; }
        private IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
        {

            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        [NeedPermission(ShopPermissions.ListProducts)]
        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new(_productCategoryApplication.GetProductCategories(), "Id", "Name");
            Products = _productApplication.Search(searchModel);
        }

        [NeedPermission(ShopPermissions.CreateProduct)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = _productCategoryApplication.GetProductCategories(true)
            };
            return Partial("./Create", command);
        }

        [NeedPermission(ShopPermissions.CreateProduct)]
        public JsonResult OnPostCreate(CreateProduct command)
        {
            var result = _productApplication.Create(command);
            return new JsonResult(result);
        }

        [NeedPermission(ShopPermissions.EditProduct)]
        public IActionResult OnGetEdit(long id)
        {
            var product = _productApplication.GetDetails(id);
            product.Categories = _productCategoryApplication.GetProductCategories(true);
            return Partial("./Edit", product);
        }

        [NeedPermission(ShopPermissions.EditProduct)]
        public JsonResult OnPostEdit(EditProduct command)
        {
            var result = _productApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
