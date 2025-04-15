using System.Diagnostics.Metrics;

namespace CafeShop.Common
{
    public static class Config
    {
        public static string Connection()
        {
            /*string conn = @"Data Source=LMK205\SQLEXPRESS;Initial Catalog=cafe;User ID=sa;Password=Leminhkhoi2003;Trust Server Certificate=True";*/

            string conn = @"Server = localhost\SQLEXPRESS;Database = cafe; Trusted_Connection = True; Trust Server Certificate=True";
            return conn;
        }

        public static string ImageUrl()
        {
            string imageUrl = @"https://localhost:44368/images/";
            return imageUrl;
        }

    }
}
