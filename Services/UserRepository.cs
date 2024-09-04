using Microsoft.Data.SqlClient;
using System.Data;
using webapisolution.Models;
using Microsoft.Extensions.Logging;

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
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.LogInformation("Database connection opened.");

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_ValidateUser";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password); // Adjust to match stored procedure parameter

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
    }
}
