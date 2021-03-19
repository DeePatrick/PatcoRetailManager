using System.Threading.Tasks;

namespace Portal.Models
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUser> Login(AuthenticationUserModel userForAuthentication);
        Task Logout();
    }
}