using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebShop.DAL.Interfaces
{
    public interface ITextBlockData
    {
        DataTable GetTextBlock(string textBlockName, string languageID);
        void SaveTextBlock(string title, string textBlock, string textBlockID, string languageID);
    }
}