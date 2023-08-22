using _01_Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permissions
{
    public class InventoryPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new()
            {
                {
                    "Inventory",
                    new()
                    {
                        new PermissionDto(InventoryPermissions.ListInventory, "مشاهده لیست انبار"),
                        new PermissionDto(InventoryPermissions.SearchInventory, "سرچ کردن در انبار"),
                        new PermissionDto(InventoryPermissions.CreateInventory, "ایحاد انبار"),
                        new PermissionDto(InventoryPermissions.EditInventory, "ویرایش انبار"),
                        new PermissionDto(InventoryPermissions.Increase, "افزایش انبار"),
                        new PermissionDto(InventoryPermissions.Reduce, "کاهش انبار"),
                        new PermissionDto(InventoryPermissions.OperationLog, "مشاهده تاریخچه انبار")
                    }
                }
            };
        }
    }
}
