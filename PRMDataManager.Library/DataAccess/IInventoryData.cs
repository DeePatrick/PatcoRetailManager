using System.Collections.Generic;

namespace PRMDataManager.Library.DataAccess
{
    public interface IInventoryData
    {
        List<InventoryModel> GetInventory();
        void SaveInventory(InventoryModel item);
    }
}