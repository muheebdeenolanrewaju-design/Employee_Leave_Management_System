using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Leave_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAllEmployees();

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(int id)
    {
        var employee = await _employeeRepository.GetEmployeeById(id);

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(
        CreateEmployeeDto dto)
    {
        var employee = await _employeeRepository.CreateEmployee(dto);

        return CreatedAtAction(
            nameof(GetEmployeeById),
            new { id = employee.Id },
            employee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Employee>> UpdateEmployee(
        int id,
        UpdateEmployeeDto dto)
    {
        var employee = await _employeeRepository.UpdateEmployee(id, dto);

        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Employee>> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.DeleteEmployee(id);

        return Ok(employee);
    }

    [HttpGet("{id}/leaves")]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetEmployeeLeaves(
        int id)
    {
        var leaves = await _employeeRepository.GetEmployeeLeaves(id);

        return Ok(leaves);
    }
}