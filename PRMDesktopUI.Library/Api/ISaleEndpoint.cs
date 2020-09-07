using PRMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace PRMDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}