using PRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
    }
}