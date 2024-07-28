using _01_Framework.Application;

namespace AccountManagement.Application.Contracts.Account
{
    public interface IAccountApplication
    {
        AccountViewModel GetAccountBy(long id);
        AccountViewModel GetAccountBy(string phoneNumber);
        OperationResult Register(RegisterAccount command);
        OperationResult Edit(EditAccount command);
        OperationResult Login(Login command);
        void Logout();
        EditAccount GetDetails(long id);
        OperationResult ChangePassword(ChangePassword command);
        List<AccountViewModel> Search(AccountSearchModel searchModel);
        List<AccountViewModel> GetAccounts();
    }
}
