using _01_Framework.Domain;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Domain.AccountAgg
{
    public class Account : EntityBase
    {
        public string Fullname { get; private set; }
        public string Password { get; private set; }
        public string Mobile { get; private set; }
        public Role Role { get; private set; }
        public long RoleId { get; private set; }
        public string ProfilePhoto { get; private set; }

        public Account(string fullname, string password, string mobile, long roleId, string profilePhoto)
        {
            Fullname = fullname;
            Password = password;
            Mobile = mobile;
            RoleId = (roleId == 0 ? 2 : roleId);
            ProfilePhoto = profilePhoto;
        }

        public void Edit(string fullname, string mobile, long roleId, string profilePhoto)
        {
            Fullname = fullname;
            Mobile = mobile;
            RoleId = roleId;

            if (!string.IsNullOrWhiteSpace(profilePhoto))
                ProfilePhoto = profilePhoto;
        }

        public void ChangePassword(string password)
        {
            Password = password;
        }
    }
}
