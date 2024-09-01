using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
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
        public async Task<ActionResult<MessageStatus>> GetEmployes()
        {
            try
            {
                var employees = _employeeRepository.GetAllEmployees();
                if (employees != null && employees.Any())
                {
                    var res = new MessageStatus
                    {
                        Message = "Data retrieved successfully.",
                        Status = true,
                        Data = employees,
                        Code = 200,

                    };

                    return Ok(res); // Return 200 OK with response object

                }
                else
                {
                    var messageStatus = new MessageStatus
                    {
                        Data = null,
                        Status = false,
                        Code = 404,
                        Message = "No data found"
                    };

                    return NotFound(messageStatus); // Return 404 Not Found with message status object
                }
            }
            catch (Exception ex)
            {
                var errorStatus = new MessageStatus
                {
                    Data = null,
                    Status = false,
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorStatus); // Return 500 Internal Server Error with message status object
            }

        }





        [HttpGet]
        [Route("SearchStudentNameById/{studentId}")]
        public async Task<ActionResult<MessageStatus>> SearchStudentNameById(int studentId)
        {
            try
            {
                 var employees =   _employeeRepository.SearchStudentNameById(studentId);

                if (employees != null && employees.Any())
                {
                    var res = new MessageStatus
                    {
                        Message = "Data retrieved successfully.",
                        Status = true,
                        Data = employees,
                        Code = 200,

                    };

                    return Ok(res); // Return 200 OK with response object
                }
                else
                {
                    var messageStatus = new MessageStatus
                    {
                        Data = null,
                        Status = false,
                        Code = 404,
                        Message = "No data found"
                    };

                    return NotFound(messageStatus); // Return 404 Not Found with message status object
                }
            }
            catch (Exception ex)
            {
                var errorStatus = new MessageStatus
                {
                    Data = null,
                    Status = false,
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorStatus); // Return 500 Internal Server Error with message status object
            }
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
