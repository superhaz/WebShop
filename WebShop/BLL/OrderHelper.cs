using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebShop.BLL.Interfaces;
using Ninject;

namespace WebShop.BLL
{
    public class OrderHelper : IOrderHelper
    {
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        #region Calculate VAT

        public string CalculateVAT(string price, string VATPercentage)
        {
            try
            {
                double priceDouble = Convert.ToDouble(price);
                double VATPercentageDouble = Convert.ToDouble(VATPercentage);
                return CalculateVAT(priceDouble, VATPercentageDouble).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public double CalculateVAT(double price, double VATPercentage)
        {
            VATPercentage = 1 + (VATPercentage / 100);
            // calculate price without VAT: 
            // formula:  x * 1.25 = totalPrice => x = totalPrice/1.25
            double priceWithoutVAT = price / VATPercentage;
            double VAT = price - priceWithoutVAT;
            return VAT;
        }

        #endregion

        #region Calculate Total Amounts

        public double CalculateTotalPrice(string sumStr, string deliverycostStr, string paymentcostStr)
        {
            double sum, deliverycost, paymentcost, totalcost;
            if (UtilHelperComponent.IsNumeric(sumStr) && UtilHelperComponent.IsNumeric(deliverycostStr) && UtilHelperComponent.IsNumeric(paymentcostStr))
            {
                sum = Convert.ToDouble(sumStr);
                deliverycost = Convert.ToDouble(deliverycostStr);
                paymentcost = Convert.ToDouble(paymentcostStr);
                totalcost = sum + deliverycost + paymentcost;
                return UtilHelperComponent.RoundPrice(totalcost);
            }
            else
            {
                return 0;
            }
        }

        public double CalculateVATOnOrderRows(DataTable orderRowsTable)
        {
            double totalVAT = 0;
            double price;
            double vatPercentage;

            foreach (DataRow row in orderRowsTable.Rows)
            {
                price = Convert.ToDouble(row["Price"].ToString());
                vatPercentage = Convert.ToDouble(row["VAT"].ToString());

                totalVAT += CalculateVAT(price, vatPercentage);
            }

            return totalVAT;
        }

        public double CalculateTotalVAT(string deliveryCost, string paymentCost, DataTable orderRowsTable)
        {
            // add VAT for delivery & payment cost
            double deliveryVAT = CalculateVAT(Convert.ToDouble(deliveryCost), 25);
            double paymentVAT = CalculateVAT(Convert.ToDouble(paymentCost), 25);
            // calculate VAT on products
            double totalProductVAT = CalculateVATOnOrderRows(orderRowsTable);
            double totalVAT = totalProductVAT + deliveryVAT + paymentVAT;
            return UtilHelperComponent.RoundPrice(totalVAT); ;
        }

        #endregion
    }
}