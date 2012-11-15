using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebShop.DAL.Interfaces;

namespace WebShop.DAL
{
    public class OrderData : IOrderData
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        #region Order Management

        public string AddProductToOrder(string orderID, string userID, string productID, string price, string vat, string quantity, string size, string color)
        {
            if (string.IsNullOrEmpty(orderID))
            {
                // create a new Order
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"DECLARE @OrderStatusID int
                                    -- get orderstatusid for shopping
                                    SELECT @OrderStatusID=OrderStatusID FROM OrderStatus WHERE OrderStatus='Shopping'
                                    INSERT INTO [Order] (OrderDate, UserID, OrderStatusID) VALUES (GETDATE(), @UserID, @OrderStatusID)
                                    SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                    if (!string.IsNullOrEmpty(userID))
                        cmd.Parameters.AddWithValue("@UserID", userID);
                    else
                        cmd.Parameters.AddWithValue("@UserID", DBNull.Value);

                    conn.Open();
                    orderID = cmd.ExecuteScalar().ToString();
                }
            }

            // add order-rows for this order
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"-- update existing order-rows or add new
                                    IF EXISTS (SELECT OrderRowID FROM OrderRow WHERE OrderID=@OrderID AND ProductID=@ProductID AND Size=@Size AND Color=@Color) 
	                                    UPDATE OrderRow SET Quantity = Quantity+@Quantity WHERE OrderID=@OrderID AND ProductID=@ProductID
                                    ELSE 
	                                    INSERT INTO OrderRow (OrderID, ProductID, Quantity, Price, VAT, Size, Color) VALUES (@OrderID, @ProductID, @Quantity, @Price, @VAT, @Size, @Color)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@VAT", vat);
                cmd.Parameters.AddWithValue("@Size", size);
                cmd.Parameters.AddWithValue("@Color", color);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return orderID;
        }

        public string GetNumOrderItemsPerOrder(string orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT SUM(Quantity) AS NumOrderItemsPerOrder
                                    FROM OrderRow
                                    WHERE (OrderID = @OrderID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                conn.Open();
                string numOrderItemsPerOrder = cmd.ExecuteScalar().ToString();

                if (string.IsNullOrEmpty(numOrderItemsPerOrder))
                    return "0";
                else
                    return numOrderItemsPerOrder;
            }
        }

        public string GetTotalOrderPrice(string orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT SUM(Quantity*Price) AS TotalOrderPrice
                                    FROM OrderRow
                                    WHERE (OrderID = @OrderID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                conn.Open();
                string totalOrderPrice = cmd.ExecuteScalar().ToString();
                if (string.IsNullOrEmpty(totalOrderPrice))
                    return "0";
                else
                    return totalOrderPrice;
            }
        }

        public DataRow GetOrderShippingInfo(string orderID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT DeliveryAddress.FirstName, DeliveryAddress.LastName, DeliveryAddress.StreetAddress, 
                                    DeliveryAddress.PostalCode, DeliveryAddress.City, DeliveryAddress.Email, DeliveryAddress.HomePhone, 
                                    DeliveryAddress.MobilePhone, DeliveryName.DeliveryName, PaymentName.PaymentName
                                    FROM [Order] INNER JOIN DeliveryAddress ON [Order].OrderID = DeliveryAddress.OrderID 
                                    INNER JOIN Delivery ON [Order].DeliveryID = Delivery.DeliveryID 
                                    INNER JOIN DeliveryName ON Delivery.DeliveryID = DeliveryName.DeliveryID 
                                    INNER JOIN Payment ON [Order].PaymentID = Payment.PaymentID 
                                    INNER JOIN PaymentName ON Payment.PaymentID = PaymentName.PaymentID
                                    WHERE ([Order].OrderID = @OrderID AND DeliveryName.LanguageID=@LanguageID AND PaymentName.LanguageID=@LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count == 1)
                    return table.Rows[0];
                else
                    return null;
            }
        }

        public DataTable GetOrder(string orderID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT [Order].OrderID, OrderRow.OrderRowID, OrderRow.Quantity, 
                                    OrderRow.Price, OrderRow.VAT, OrderRow.Size, OrderRow.Color, Product.ProductID, OrderRow.Quantity * OrderRow.Price AS Sum,
                                        -- subqueries for getting title and image filename
                                        (SELECT Title FROM ProductTitle WHERE (LanguageID = @LanguageID) AND (ProductID = OrderRow.ProductID)) AS ProductTitle,
                                        (SELECT TOP 1 [File].FileName FROM  ProductFile INNER JOIN [File] ON ProductFile.FileID = [File].FileID
                                         WHERE ProductID = Product.ProductID ORDER BY ProductFile.Priority DESC) AS FileName
                                    FROM  [Order] INNER JOIN
                                    OrderRow AS OrderRow ON [Order].OrderID = OrderRow.OrderID INNER JOIN
                                    Product ON OrderRow.ProductID = Product.ProductID
                                    WHERE [Order].OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public void IncreaseOrderRowQuantity(string orderRowID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"UPDATE OrderRow SET Quantity = Quantity+1 WHERE OrderRowID=@OrderRowID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderRowID", orderRowID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string DecreaseOrderRowQuantity(string orderRowID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"-- check if order row has more than one quantity if not remove order-row
                                    DECLARE @OrderID int
                                    SELECT @OrderID=OrderID FROM OrderRow WHERE OrderRowID=@OrderRowID

                                    IF(SELECT Quantity FROM OrderRow WHERE OrderRowID=@OrderRowID) > 1
	                                    UPDATE OrderRow SET Quantity = Quantity-1 WHERE OrderRowID=@OrderRowID
                                    ELSE
	                                    DELETE FROM OrderRow WHERE OrderRowID=@OrderRowID

                                    -- remove orders that doesn't have any order-rows
                                    IF(SELECT COUNT(OrderID) FROM OrderRow WHERE OrderID=@OrderID)<1
	                                    DELETE FROM [Order] WHERE OrderID=@OrderID
                    
                                    SELECT COUNT(OrderID) FROM OrderRow WHERE OrderID=@OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderRowID", orderRowID);

                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
        }

        public bool VerifyThatOrderExists(string orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT OrderID FROM [Order] WHERE OrderID=@OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result == null)
                    return false;
                else
                    return true;

            }
        }

        #region setting OrderStatus FUTURE USAGE
        //        public void SetOrderStatusToOrderReceived(string orderID)
//        {
//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                string sqlQuery = @"-- get the orderstatusID for OrderReceived ie order has been ordered by customer
//                                    DECLARE @OrderStatusID int
//                                    SELECT @OrderStatusID=OrderStatusID FROM OrderStatus WHERE OrderStatus='OrderReceived'
//                                    -- update order with status and date
//                                    UPDATE [Order] SET OrderDate = GETDATE(), OrderStatusID=@OrderStatusID WHERE OrderID=@OrderID";

//                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
//                cmd.Parameters.AddWithValue("@OrderID", orderID);

//                conn.Open();
//                cmd.ExecuteNonQuery();
//            }
        //        }
        #endregion

        public void CompleteOrder(string orderID, string firstName, string lastName, string streetAddress, string postalCode, string city, string email, string homePhone, string mobilePhone)
        {
            // add handle code for:
            /// possible exception error: Violation of PRIMARY KEY constraint 'PK_DeliveryAddress'. Cannot insert duplicate key in object 'dbo.DeliveryAddress'. The statement has been terminated. 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"-- change orderstatus to OrderReceieved ie that the order has been completed
                                    DECLARE @OrderStatusID int
                                    SELECT @OrderStatusID=OrderStatusID FROM OrderStatus WHERE OrderStatus='OrderReceived'
                                    -- update order with status and date
                                    UPDATE [Order] SET OrderDate = GETDATE(), OrderStatusID=@OrderStatusID WHERE OrderID=@OrderID
                                    -- add customer delivery address to order
                                    INSERT INTO DeliveryAddress (OrderID, FirstName, LastName, StreetAddress, PostalCode, City, Email, HomePhone, MobilePhone) 
                                    VALUES 
                                    (@OrderID, @FirstName, @LastName, @StreetAddress, @PostalCode, @City, @Email, @HomePhone, @MobilePhone)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@StreetAddress", streetAddress);
                cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@HomePhone", homePhone);
                cmd.Parameters.AddWithValue("@MobilePhone", mobilePhone);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Payment & Delivery

        public DataTable GetDeliveries(string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Delivery.DeliveryID, DeliveryName.DeliveryNameID, DeliveryName.DeliveryName, 
                                    Delivery.Price, Delivery.ImageUrl, DeliveryName.DeliveryDescription
                                    FROM Delivery INNER JOIN DeliveryName ON Delivery.DeliveryID = DeliveryName.DeliveryID
                                    WHERE (DeliveryName.LanguageID = @LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public DataRow GetDeliveryByDeliveryName(string languageID, string deliveryName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT DISTINCT Delivery.DeliveryID, Delivery.Price, DeliveryName.DeliveryName, DeliveryName.DeliveryDescription FROM Delivery
                                    INNER JOIN DeliveryName ON Delivery.DeliveryID = DeliveryName.DeliveryID
                                    WHERE (LanguageID = @LanguageID) AND (DeliveryName = @DeliveryName)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                cmd.Parameters.AddWithValue("@DeliveryName", deliveryName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count == 1)
                    return table.Rows[0];
                else
                    return null;
            }
        }

        public DataRow GetPaymentByPaymentNameInfo(string languageID, string paymentName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT DISTINCT Payment.PaymentID, Payment.Price, PaymentName.PaymentName, PaymentName.PaymentDescription FROM Payment
                                    INNER JOIN PaymentName ON Payment.PaymentID = PaymentName.PaymentID
                                    WHERE (LanguageID = @LanguageID) AND (PaymentName = @PaymentName)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                cmd.Parameters.AddWithValue("@PaymentName", paymentName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count == 1)
                    return table.Rows[0];
                else
                    return null;
            }
        }

        public DataTable GetPayments(string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Payment.PaymentID, PaymentName.PaymentNameID, PaymentName.PaymentName, 
                                    Payment.Price, Payment.ImageUrl, PaymentName.PaymentDescription
                                    FROM Payment INNER JOIN PaymentName ON Payment.PaymentID = PaymentName.PaymentID
                                    WHERE (PaymentName.LanguageID = @LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public void SetDeliveryForOrder(string orderID, string deliveryName, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"-- get the id of the DeliveryName
                                    DECLARE @DeliveryID int, @DeliveryCost smallmoney
                                    SELECT DISTINCT @DeliveryID=Delivery.DeliveryID, @DeliveryCost=Delivery.Price FROM Delivery
                                    INNER JOIN DeliveryName ON Delivery.DeliveryID = DeliveryName.DeliveryID
                                    WHERE (LanguageID = @LanguageID) AND (DeliveryName = @DeliveryName)
                                    -- add the deliveryid and cost to Order
                                    UPDATE [Order] SET DeliveryID=@DeliveryID, DeliveryCost=@DeliveryCost WHERE OrderID=@OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@DeliveryName", deliveryName);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void SetPaymentForOrder(string orderID, string paymentName, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"-- get the id of the DeliveryName
                                    DECLARE @PaymentID int, @PaymentCost smallmoney
                                    SELECT DISTINCT @PaymentID=Payment.PaymentID, @PaymentCost=Payment.Price FROM Payment
                                    INNER JOIN PaymentName ON Payment.PaymentID = PaymentName.PaymentID
                                    WHERE (LanguageID = @LanguageID) AND (PaymentName = @PaymentName)
                                    -- add the deliveryid and cost to Order
                                    UPDATE [Order] SET PaymentID=@PaymentID, PaymentCost=@PaymentCost WHERE OrderID=@OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@PaymentName", paymentName);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string GetOrderDeliveryCost(string orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT DeliveryCost FROM [Order] WHERE OrderID=@OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();
                if (string.IsNullOrEmpty(result))
                    return "0";
                return result;
            }
        }

        public string GetOrderPaymentCost(string orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT PaymentCost FROM [Order] WHERE OrderID=@OrderID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();
                if (string.IsNullOrEmpty(result))
                    return "0";
                return result;
            }
        }

        #endregion

    }
}
