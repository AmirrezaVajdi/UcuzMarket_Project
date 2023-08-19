namespace _01_Framework.Application
{
    public interface IAuthHelper
    {
        bool IsAuthenticated();
        void Signin(AuthViewModel account);
        void SignOut();
        string CurrenAccountRole();
        AuthViewModel CurrentAccountInfo();
    }
}
