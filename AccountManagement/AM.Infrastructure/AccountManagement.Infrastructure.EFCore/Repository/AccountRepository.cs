using _01_Framework.Application;
using _01_Framework.Domain;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository : RepositoryBase<long, Account>, IAccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context) : base(context)
        {
            _context = context;
        }

        public List<AccountViewModel> GetAccounts()
        {
            return _context.Accounts.Select(x => new AccountViewModel
            {
                Id = x.Id,
                Fullname = x.Fullname
            }).ToList();
        }

        public Account GetBy(string phoneNumber)
        {
            return _context.Accounts.FirstOrDefault(x => x.Mobile == phoneNumber);
        }

        public EditAccount GetDetails(long id)
        {
            return _context.Accounts
                  .Select(x => new EditAccount
                  {
                      Id = x.Id,
                      Fullname = x.Fullname,
                      Mobile = x.Mobile,
                      RoleId = x.RoleId,
                  }).FirstOrDefault(x => x.Id == id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            var query = _context.Accounts
                .Include(x => x.Role)
                .Select(x => new AccountViewModel
                {
                    Id = x.Id,
                    Fullname = x.Fullname,
                    Mobile = x.Mobile,
                    ProfilePhoto = x.ProfilePhoto,
                    Role = x.Role.Name,
                    RoleId = x.RoleId,
                    CreationDate = x.CreationDate.ToFarsi()
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
                query = query.Where(x => x.Fullname.Contains(searchModel.Fullname));

            if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
                query = query.Where(x => x.Mobile.Contains(searchModel.Mobile));

            if (searchModel.RoleId > 0)
                query = query.Where(x => x.RoleId == searchModel.RoleId);

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
