using System.Collections.Generic;
using webapisolution.Models;

namespace webapisolution.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        List<Employee>SearchStudentNameById (int studentId);
    }
}
