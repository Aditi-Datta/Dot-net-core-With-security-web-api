using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EmployeController> _logger;


        public EmployeController(ILogger<EmployeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;

            _employeeRepository = employeeRepository;
        }

    

        [HttpGet]
        [Route("getAllEmployes")]
        [Authorize]
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



        [HttpPost]
        [Route("InsertStudents")]
        public async Task<ActionResult<MessageStatus>> InsertStudents([FromBody] List<Student> students)
        {
            if (students == null || !students.Any())
            {
                var badRequestStatus = new MessageStatus
                {
                    Data = null,
                    Status = false,
                    Code = 400,
                    Message = "No student data provided."
                };

                return BadRequest(badRequestStatus);
            }

            try
            {
                var insertedStudents = new List<Student>();

                foreach (var student in students)
                {
                    var insertedStudent = _employeeRepository.InsertStudentInfo(student);
                    if (insertedStudent != null && insertedStudent.Any())
                    {
                        insertedStudents.AddRange(insertedStudent);
                    }
                }

                if (insertedStudents.Any())
                {
                    var successStatus = new MessageStatus
                    {
                        Message = "Students inserted successfully.",
                        Status = true,
                        Data = insertedStudents,
                        Code = 201
                    };

                    return Ok(successStatus); // Return 201 Created with message status object
                }
                else
                {
                    var errorStatus = new MessageStatus
                    {
                        Data = null,
                        Status = false,
                        Code = 500,
                        Message = "An error occurred while inserting the students."
                    };

                    return StatusCode((int)HttpStatusCode.InternalServerError, errorStatus);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during InsertStudents: {Message}", ex.Message);

                var errorStatus = new MessageStatus
                {
                    Data = null,
                    Status = false,
                    Code = 500,
                    Message = $"Internal server error: {ex.Message}"
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorStatus);
            }
        }






    }
}
