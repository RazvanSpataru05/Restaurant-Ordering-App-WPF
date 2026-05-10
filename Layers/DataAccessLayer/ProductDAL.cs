using Microsoft.Data.SqlClient;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Data;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class ProductDAL
    {
        public ObservableCollection<Product> GetAllProducts()
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetAllProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            ObservableCollection<Product> result = [];
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(
                        new Product()
                        {
                            ProductId = (int)reader["ProductId"],
                            Name = (string)reader["Name"],
                            Price = (decimal)reader["Price"],
                            PortionQuantity = (string)reader["PortionQuantity"],
                            TotalQuantity = (decimal)reader["TotalQuantity"],
                            CategoryId = (int)reader["CategoryId"],
                            IsAvailable = (bool)reader["IsAvailable"],
                            CategoryName = (string)reader["CategoryName"]
                        }
                    );
                }
            }
            return result;
        }
        public ObservableCollection<Product> GetProductsByCategory(int categoryId)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetProductsByCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            con.Open();
            ObservableCollection<Product> result = [];
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(
                        new Product()
                        {
                            ProductId = (int)reader["ProductId"],
                            Name = (string)reader["Name"],
                            Price = (decimal)reader["Price"],
                            PortionQuantity = (string)reader["PortionQuantity"],
                            TotalQuantity = (decimal)reader["TotalQuantity"],
                            CategoryId = (int)reader["CategoryId"],
                            IsAvailable = (bool)reader["IsAvailable"],
                            CategoryName = (string)reader["CategoryName"]
                        }
                    );
                }
            }
            return result;
        }
        public void UpdateProductQuantity(int productId, decimal totalQuantity)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_UpdateProductQuantity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@TotalQuantity", totalQuantity);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
