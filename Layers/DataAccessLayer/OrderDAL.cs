using Microsoft.Data.SqlClient;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Data;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class OrderDAL
    {
        public (int, string) CreateOrder(int userId, DateTime estimatedDeliveryTime, decimal totalPrice, decimal deliveryCost)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_CreateOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryTime", estimatedDeliveryTime);
            cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
            cmd.Parameters.AddWithValue("@DeliveryCost", deliveryCost);

            SqlParameter outputId = new("@OrderId", SqlDbType.Int);
            SqlParameter outputCode = new("@OrderCode", SqlDbType.NVarChar, 50);
            outputId.Direction = ParameterDirection.Output;
            outputCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputId);
            cmd.Parameters.Add(outputCode);

            con.Open();
            cmd.ExecuteNonQuery();
            return ((int)outputId.Value, outputCode.Value.ToString());
        }
        public ObservableCollection<Order> GetOrdersByUser(int userId)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetOrdersByUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            con.Open();
            ObservableCollection<Order> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Order
                {
                    OrderCode = (string)reader["OrderCode"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    EstimatedDeliveryTime = (DateTime)reader["EstimatedDeliveryTime"],
                    TotalPrice = (decimal)reader["TotalPrice"],
                    Status = (string)reader["Status"]
                });
            }
            return result;
        }
        public void UpdateOrderStatus(int orderId, string status)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_UpdateOrderStatus", con);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@Status", status);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public void AddOrderItem(int orderId, int? productId, int? menuId, int quantity, decimal unitPrice)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_AddOrderItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@ProductId", productId == null ? DBNull.Value : productId);
            cmd.Parameters.AddWithValue("@MenuId", menuId == null ? DBNull.Value : menuId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
