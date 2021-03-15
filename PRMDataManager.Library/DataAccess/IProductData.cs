using PRMDataManager.Library.Models;
using System.Collections.Generic;

namespace PRMDataManager.Library.DataAccess
{
    public interface IProductData
    {
        ProductModel GetProductById(int productId);
        List<ProductModel> GetProducts();
    }
}