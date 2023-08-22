namespace _01_Framework.Application
{
    public class NeedPermissionAttribute : Attribute
    {
        public int Permission { get; set; }

        public NeedPermissionAttribute(int permission)
        {
            Permission = permission;
        }
    }
}
