using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantOrderingApp.Layers.EntityLayer;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class MenuDAL
    {
        public ObservableCollection<Menu> GetAllMenus()
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetAllMenus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            ObservableCollection<Menu> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Menu()
                {
                    MenuId = (int)reader["MenuId"],
                    Name = (string)reader["Name"],
                    CategoryId = (int)reader["CategoryId"],
                    DiscountPercent = (decimal)reader["Discount"]
                });
            }
            return result;
        }
        public ObservableCollection<Product> GetMenuProducts(int menuId)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetMenuProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MenuId", menuId);
            con.Open();
            ObservableCollection<Product> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Product()
                {
                    ProductId = (int)reader["ProductId"],
                    Name = (string)reader["Name"],
                    PortionQuantity = (string)reader["PortionQuantity"],
                    Price = (decimal)reader["Price"]
                });
            }
            return result;
        }
    }
}
