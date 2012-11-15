using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WebShop.UserControls.FileImageManager.DAL.DataProviders
{
    public interface IItemDataProvider
    {
        #region Get Methods
        DataTable GetItemFiles(string itemID);

        DataTable GetFilesByItemID(string itemID);
        #endregion

        #region Priority Management
        DataTable GetFileByItemIDANDPriority(string id, string priority);

        void SetPriorityByItemFileID(string itemFileID, string highestPrio);
        
        string GetANewHighestPriority(string itemID);

        string GetANewLowestPriority(string itemID);

        void IncreasePriority(string itemFileID, string itemID);

        void DecreasePriority(string itemFileID, string itemID);
        #endregion

        #region Save & Delete
        void SaveItemFile(string fileID, string selectedID);

        void DeleteItemFileByFileID(string fileID);
        #endregion

    }


}
