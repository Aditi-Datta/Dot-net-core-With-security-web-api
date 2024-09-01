using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using webapisolution.Models;
using webapisolution.Repositories;

namespace webapisolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("getAllEmployes")]
        public JsonResult GetEmployes()
        {
            List<Employee> employes = _employeeRepository.GetAllEmployees();
            return new JsonResult(employes);
        }


        [HttpGet]
        [Route("SearchStudentNameById")]
        public JsonResult SearchStudentNById(int studentId)
        {
            List<Employee> employes = _employeeRepository.SearchStudentNameById(studentId);
            return new JsonResult(employes);
        }


        [HttpDelete]
        [Route("DeleteStudentById")]
        public JsonResult DeleteStudentById(int studentId)
        {
            List<Employee> employes = _employeeRepository.DeleteStudentById(studentId);
            return new JsonResult(employes);
        }


    }
}
