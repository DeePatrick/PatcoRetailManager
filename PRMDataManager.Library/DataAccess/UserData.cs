using PRMDataManager.Library.Internal.DataAccess;
using PRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { pId = Id };
            var output = sql.LoadData<UserModel, dynamic>("[dbo].[spUserLookUp]", p, "PRMData");
            return output;
        }
    }
}
