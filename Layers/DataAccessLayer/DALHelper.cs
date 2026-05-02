using System.Configuration;
using Microsoft.Data.SqlClient;

namespace RestaurantOrderingApp.Layers.DataAccessLayer
{
    public class DALHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["myConStr"].ConnectionString;

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}
