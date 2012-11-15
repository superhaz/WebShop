using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebShop.BLL.Interfaces
{
    public interface IOrderHelper
    {
        #region Calculate VAT

        string CalculateVAT(string price, string VATPercentage);
        double CalculateVAT(double price, double VATPercentage);

        #endregion

        #region Calculate Total Amounts

        double CalculateTotalPrice(string sumStr, string deliverycostStr, string paymentcostStr);
        double CalculateVATOnOrderRows(DataTable orderRowsTable);
        double CalculateTotalVAT(string deliveryCost, string paymentCost, DataTable orderRowsTable);

        #endregion
    }
}