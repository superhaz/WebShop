using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebShop.DAL.Interfaces
{
    public interface IOrderData
    {
        #region Order Management

        string AddProductToOrder(string orderID, string userID, string productID, string price, string vat, string quantity, string size, string color);
        string GetNumOrderItemsPerOrder(string orderID);
        string GetTotalOrderPrice(string orderID);
        DataRow GetOrderShippingInfo(string orderID, string languageID);
        DataTable GetOrder(string orderID, string languageID);
        void IncreaseOrderRowQuantity(string orderRowID);
        string DecreaseOrderRowQuantity(string orderRowID);
        bool VerifyThatOrderExists(string orderID);
        void CompleteOrder(string orderID, string firstName, string lastName, string streetAddress, string postalCode, string city, string email, string homePhone, string mobilePhone);
        #region setting OrderStatus FUTURE USAGE
        //        void SetOrderStatusToOrderReceived(string orderID);
        #endregion

        #endregion

        #region Payment & Delivery

        DataTable GetDeliveries(string languageID);
        DataRow GetDeliveryByDeliveryName(string languageID, string deliveryName);
        DataRow GetPaymentByPaymentNameInfo(string languageID, string paymentName);
        DataTable GetPayments(string languageID);
        void SetDeliveryForOrder(string orderID, string deliveryName, string languageID);
        void SetPaymentForOrder(string orderID, string paymentName, string languageID);
        string GetOrderDeliveryCost(string orderID);
        string GetOrderPaymentCost(string orderID);

        #endregion
    }
}