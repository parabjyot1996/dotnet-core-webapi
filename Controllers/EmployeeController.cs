using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Dtos;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

namespace NetCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employees.
        /// </summary>
        /// <returns>Returns all employees</returns>
        /// <response code="200">Returns all employees</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            var employees = _repo.GetAllEmployees();
            //AutoMapper used to convert Domain model to Dto model
            var employeesDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        /// <summary>
        /// Gets a specific employee.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a specific employee</returns>
        /// <response code="200">Returns a specific employee</response>
        /// <response code="404">If employee not found</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmployee(int id)
        {
            var employee = _repo.GetEmployee(id);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            if (employeeDto == null)
                return NotFound($"Employee not found with Id { id }");

            return Ok(employeeDto);
        }

        /// <summary>
        /// Creates an employee.
        /// </summary>
        /// <param name="emp"></param>
        /// <returns>Returns newly created employee</returns>
        /// <response code="200">Create a new employee</response>
        /// <response code="400">If data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostEmployee(EmployeeDto emp)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(emp);
                int empId = _repo.CreateEmployee(employee);

                return Created(new Uri($"{ Request.Path }/{ empId }", UriKind.Relative), emp);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a specific employee.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Deletes specific employee</response>
        /// <response code="404">If employee not found</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEmployee(int id)
        {
            int result = _repo.DeleteEmployee(id);

            if (result == -1)
                return NotFound($"Employee with Id { id } not found");

            return Ok("Employee delete successfully");
        }

        /// <summary>
        /// Updates a specific employee.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="empDto"></param>
        /// <response code="200">Updates specific employee</response>
        /// <response code="400">If data is invalid</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEmployee(int id, EmployeeDto empDto)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(empDto);
                int result = _repo.UpdateEmployee(id, employee);

                return Ok("Employee updated successfully");
            }

            return BadRequest(ModelState);
        }
    }
}