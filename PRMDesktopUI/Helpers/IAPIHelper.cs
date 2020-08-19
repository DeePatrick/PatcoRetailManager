using PRMDesktopUI.Models;
using System.Threading.Tasks;

namespace PRMDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}