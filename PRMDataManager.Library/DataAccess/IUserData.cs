using PRMDataManager.Library.Models;
using System.Collections.Generic;

namespace PRMDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}