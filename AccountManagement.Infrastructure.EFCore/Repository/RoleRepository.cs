using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class RoleRepository : RepositoryBase<long, Role>, IRoleRepository
    {
        private readonly AccountContext _context;

        public RoleRepository(AccountContext context) : base(context)
        {
            _context = context;
        }

        public EditRole GetDetails(long id)
        {
            return _context.Roles.Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MappingPermissions(x.Permissions)
            }).AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        private static List<PermissionDto> MappingPermissions(List<Permission> permissions)
        {
            return permissions.Select(x => new PermissionDto(x.Code, x.Name)).ToList();
        }

        public List<RoleViewModel> List()
        {
            return _context.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi()
            }).ToList();
        }
    }
}
