using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.BLL.Interfaces
{
    public interface IUtilHelper
    {
        #region Rounding price

        string RoundPrice(string price);
        double RoundPrice(double price);

        #endregion

        #region Random

        int RandomNumber(int max);

        #endregion

        #region Convert Size KB/MB/GB

        string ConvertSizeToKBOrMBOrGBOrTB(string sizeString);

        #endregion

        #region IsInteger & IsNumeric

        bool IsNumeric(string stringToTest);
        bool IsInteger(string stringToTest);

        #endregion

        #region string convertion

        string ParseNewLine(object text);
        string URLEncode(string s);

        #endregion

        #region string manipulation

        int FindLastIndexOf(string str, string searchString);
        string RemoveLastSearchString(string str, string searchString);

        #endregion
    }
}