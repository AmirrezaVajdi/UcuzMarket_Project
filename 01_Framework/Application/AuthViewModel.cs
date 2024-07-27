namespace _01_Framework.Application
{
    public class AuthViewModel
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        public long RoleId { get; set; }
        public List<int> Permissions { get; set; }

        public AuthViewModel()
        {

        }
        public AuthViewModel(long id, long roleId, string fullname, List<int> permissions)
        {
            Id = id;
            RoleId = roleId;
            Fullname = fullname;
            Permissions = permissions;
        }
    }
}