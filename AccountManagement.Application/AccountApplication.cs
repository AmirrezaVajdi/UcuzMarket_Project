using _0_Framework.Application;
using _01_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthHelper _authHelper;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader, IAuthHelper authHelper)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _authHelper = authHelper;
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            OperationResult operation = new();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordNotMath);

            var password = _passwordHasher.Hash(command.Password);

            account.ChangePassword(password);
            _accountRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Register(RegisterAccount command)
        {
            OperationResult operation = new();
            if (_accountRepository.Exsists(x => x.Username == command.Username || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);

            var password = _passwordHasher.Hash(command.Password);
            var path = "ProfilePhotos";
            var pictureName = _fileUploader.Upload(command.ProfilePhoto, path);

            Account account = new(command.Fullname, command.Username, password, command.Mobile, command.RoleId, pictureName);
            _accountRepository.Create(account);
            _accountRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditAccount command)
        {
            OperationResult operation = new();
            var account = _accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_accountRepository.Exsists(x =>
                (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecored);


            var path = "ProfilePhotos";
            var pictureName = _fileUploader.Upload(command.ProfilePhoto, path);

            account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, pictureName);
            _accountRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public OperationResult Login(Login command)
        {
            OperationResult operation = new();
            var account = _accountRepository.GetBy(command.Username);
            if (account == null)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            (bool Verified, bool NeedsUpgrade) = _passwordHasher.Check(account.Password, command.Password);

            if (!Verified)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            AuthViewModel authViewModel = new(account.Id, account.RoleId, account.Fullname, account.Username);

            _authHelper.Signin(authViewModel);

            return operation.Succeded();
        }

        public void Logout()
        {
            _authHelper.SignOut();
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}
