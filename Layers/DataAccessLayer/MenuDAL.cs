using Microsoft.Data.SqlClient;
using RestaurantOrderingApp.Display;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class MenuDAL
    {
        private readonly decimal _menuDiscount = Convert.ToDecimal(ConfigurationManager.AppSettings["MenuDiscount"]);
        public ObservableCollection<MenuDisplay> GetAllMenus()
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetAllMenus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            ObservableCollection<MenuDisplay> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new MenuDisplay()
                {
                    MenuEntity = new Menu
                    {
                        MenuId = (int)reader["MenuId"],
                        Name = (string)reader["Name"],
                        CategoryId = (int)reader["CategoryId"],
                        DiscountPercent = (decimal)reader["DiscountPercent"],
                        ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"]
                    },
                    CalculatedPrice = Math.Floor((decimal)reader["CalculatedPrice"] * (1 - _menuDiscount))
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
        public void ClearMenuProducts(int menuId)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_ClearMenuProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MenuId", menuId);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public void AddMenuProduct(int menuId, int productId, string portionQuantity)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_AddMenuProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MenuId", menuId);
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@PortionQuantity", portionQuantity);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
