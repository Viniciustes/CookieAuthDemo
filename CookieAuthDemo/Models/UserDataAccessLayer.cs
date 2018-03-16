using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CookieAuthDemo.Models
{
    public class UserDataAccessLayer
    {
        private string connectionString = GetConnectionString();
        private static IConfiguration Configuration { get; set; }

        private static string GetConnectionString()
        {
            var buider = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = buider.Build();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            return connectionString;
        }

        public string RegisterUser(UserDetails user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("spRegisterUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);

                connection.Open();
                string result = command.ExecuteScalar().ToString();
                connection.Close();
                return result;
            }
        }

        public string ValidateLogin(UserDetails user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("spValidateUserLogin", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@LoginId", user.UserId);
                command.Parameters.AddWithValue("@LoginPassword", user.UserPassword);

                connection.Open();
                string result = command.ExecuteScalar().ToString();
                connection.Close();
                return result;
            }
        }
    }
}