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
        public ObservableCollection<MenuProduct> GetMenuProducts(int menuId)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetMenuProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MenuId", menuId);
            con.Open();
            ObservableCollection<MenuProduct> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new MenuProduct()
                {
                    MenuId = (int)reader["MenuId"],
                    ProductId = (int)reader["ProductId"],
                    PortionQuantity = (string)reader["PortionQuantity"],
                    TotalQuantity = (decimal)reader["TotalQuantity"],
                    Price = (decimal)reader["Price"]
                });
            }
            return result;
        }
    }
}
