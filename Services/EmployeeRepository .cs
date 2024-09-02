using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using webapisolution.Models;

namespace webapisolution.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<EmployeeRepository> _logger;


        public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger)

        {
            _connectionString = configuration.GetConnectionString("EmployeAppCon");
            _logger = logger;
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
                                salary = reader.GetDecimal(reader.GetOrdinal("result"))
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
                                salary = reader.GetDecimal(reader.GetOrdinal("result"))
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
                    cmd.CommandText = "dbo.[DeleteStudentById]"; // Assuming this is a stored procedure name
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
                                salary = reader.GetDecimal(reader.GetOrdinal("result"))
                            };

                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees;
        }



        public List<Student> InsertStudentInfo(Student student)
        {
            List<Student> students = new List<Student>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.LogInformation("Connection to the database opened successfully.");

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.InsertEmployee"; // Stored procedure name
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding parameters to the stored procedure
                        cmd.Parameters.AddWithValue("@FirstName", student.Name);
                        cmd.Parameters.AddWithValue("@cgpa", student.cgpa);

                        // Execute the insert command and get the newly inserted stuId
                        int newId = Convert.ToInt32(cmd.ExecuteScalar());
                        _logger.LogInformation("New student inserted with ID: {NewStudentId}", newId);

                        // Retrieve the newly inserted student record by stuId
                        cmd.CommandText = "SELECT * FROM studentResultTbl WHERE stuId = @StudentId";
                        cmd.CommandType = CommandType.Text; // Set command type to text for the select query
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@StudentId", newId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Student newStudent = new Student
                                {
                                    StudentId = reader.GetInt32(reader.GetOrdinal("stuId")),
                                    Name = reader.GetString(reader.GetOrdinal("stuName")),
                                    cgpa = reader.GetDecimal(reader.GetOrdinal("result"))
                                };

                                students.Add(newStudent);
                            }
                        }

                        _logger.LogInformation("Student data retrieved successfully for ID: {NewStudentId}", newId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during InsertStudentInfo: {Message}", ex.Message);
                throw;
            }

            return students;
        }




    }


}
