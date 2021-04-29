using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PRMDataManager.Library.Internal.DataAccess;
using PRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {

        private readonly ISqlDataAccess _sql;
        private readonly ILogger<UserData> _logger;

        public UserData(ISqlDataAccess sql, ILogger<UserData> logger)
        {
            _sql = sql;
            _logger = logger;
        }
        public List<UserModel> GetUserById(string Id)
        {
            var p = new { pId = Id };
            var output = _sql.LoadData<UserModel, dynamic>("[dbo].[spUserLookUp]", p, "PRMData");
            return output;
        }

        public void SaveUser(UserModel userInfo)
        {
            try
            {
                _sql.SaveData("[dbo].[spUser_Insert]", userInfo, "PRMData");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, null);
            }
        }
    }
}

