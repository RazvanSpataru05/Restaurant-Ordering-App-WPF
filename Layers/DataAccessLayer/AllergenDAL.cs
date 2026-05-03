using Microsoft.Data.SqlClient;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class AllergenDAL
    {
        public ObservableCollection<Allergen> GetAllAllergens()
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetAllAllergens", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            ObservableCollection<Allergen> result = [];
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Allergen
                    {
                        AllergenId = (int)reader["AllergenId"],
                        Name = (string)reader["Name"],
                    });
                }
            }
            return result;
        }
        public ObservableCollection<Allergen> GetProductAllergens(int productId)
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetProductAllergens", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductId", productId);
            con.Open();
            ObservableCollection<Allergen> result = [];
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Allergen
                    {
                        AllergenId = (int)reader["AllergenId"],
                        Name = (string)reader["Name"],
                    });
                }
            }
            return result;
        }
        public Dictionary<int, ObservableCollection<Allergen>> GetAllProductAllergens()
        {
            using SqlConnection con = DALHelper.Connection;
            SqlCommand cmd = new("sp_GetAllProductAllergens", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            Dictionary<int, ObservableCollection<Allergen>> result = [];
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!result.ContainsKey((int)reader["ProductId"]))
                {
                    result[(int)reader["ProductId"]] = [];
                }
                result[(int)reader["ProductId"]].Add(new Allergen
                {
                    AllergenId = (int)reader["AllergenId"],
                    Name = (string)reader["Name"],
                });
            }
            return result;
        }
    }
}
