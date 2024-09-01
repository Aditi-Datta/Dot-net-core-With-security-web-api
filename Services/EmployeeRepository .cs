using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using webapisolution.Models;

namespace webapisolution.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EmployeAppCon");
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.datalist"; // Assuming this is a stored procedure name
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                EmployeeId = reader.GetInt32(reader.GetOrdinal("stuId")),
                                FirstName = reader.GetString(reader.GetOrdinal("stuName")),
                                Phone = reader.GetString(reader.GetOrdinal("result"))
                                // Map other columns as needed
                            };

                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }
    }
}
