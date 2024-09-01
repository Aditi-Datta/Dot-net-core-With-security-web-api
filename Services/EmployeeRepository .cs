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
                                cgpa = reader.GetString(reader.GetOrdinal("result"))
                             };

                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }



    public List<Employee> SearchStudentNameById(int studentId)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetStudentNameById"; // Assuming this is a stored procedure name
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adding the studentId as a parameter to the stored procedure
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                EmployeeId = reader.GetInt32(reader.GetOrdinal("stuId")),
                                FirstName = reader.GetString(reader.GetOrdinal("stuName")),
                                cgpa = reader.GetString(reader.GetOrdinal("result"))
                            };

                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees;
        }



        public List<Employee> DeleteStudentById(int studentId)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.DeleteStudentById"; // Assuming this is a stored procedure name
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adding the studentId as a parameter to the stored procedure
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                EmployeeId = reader.GetInt32(reader.GetOrdinal("stuId")),
                                FirstName = reader.GetString(reader.GetOrdinal("stuName")),
                                cgpa = reader.GetString(reader.GetOrdinal("result"))
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
