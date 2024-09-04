using Microsoft.Data.SqlClient;
using System.Data;
using webapisolution.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace webapisolution.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("EmployeAppCon");
            _logger = logger;
        }

        public User ValidateUser(string username, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.LogInformation("Database connection opened.");

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_ValidateUser";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        _logger.LogInformation("Executing stored procedure sp_ValidateUser.");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _logger.LogInformation("User found in the database.");
                                return new User
                                {
                                    Username = reader["Username"].ToString()
                                };
                            }
                            else
                            {
                                _logger.LogWarning("No user found with the provided credentials.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user");
            }
            return null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
