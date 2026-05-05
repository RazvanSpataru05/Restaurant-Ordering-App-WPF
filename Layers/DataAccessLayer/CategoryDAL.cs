using Microsoft.Data.SqlClient;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Data;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class CategoryDAL
    {
        public ObservableCollection<Category> GetAllCategories()
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetAllCategories", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            ObservableCollection<Category> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Category()
                {
                    CategoryId = (int)reader["CategoryId"],
                    Name = (string)reader["Name"],
                });
            }
            return result;
        }
    }
}
