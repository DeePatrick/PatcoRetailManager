using PRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRMDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}

