using System.Collections.Generic;
using webapisolution.Models;

namespace webapisolution.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        List<Employee>SearchStudentNameById (int studentId);

        List<Employee> DeleteStudentById(int studentId);
        List<Student> InsertStudentInfo(Student student);


    }
}
