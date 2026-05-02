using RestaurantOrderingApp.Layers.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class UserDAL
    {
        public User? AuthenticateUser(string email, string passwordHash)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("sp_AuthenticateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User user = new()
                        {
                            UserId = (int)reader["UserId"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Email = email,
                            Phone = (string)reader["Phone"],
                            DeliveryAddress = (string)reader["DeliveryAddress"],
                            PasswordHash = passwordHash,
                            Role = (string)reader["Role"]
                        };
                        return user;
                    }
                }
            }
            return null;
        }
        public bool RegisterUser(User user)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_RegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@DeliveryAddress", user.DeliveryAddress);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex) when (ex is SqlException)
                {
                    return false;
                }
            }
        }
    }
}
