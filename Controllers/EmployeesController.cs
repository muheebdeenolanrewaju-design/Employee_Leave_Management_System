using Employee_Leave_Management_System.Models.Dtos.Requests;
using Employee_Leave_Management_System.Repositories.Interface;
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

    // GET all employees
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var result = await _employeeRepository.GetAllEmployees();
        return Ok(result);
    }

    // GET employee by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var result = await _employeeRepository.GetEmployeeById(id);
        return Ok(result);
    }

    // create employee
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequestDto dto)
    {
        var result = await _employeeRepository.CreateEmployee(dto);
        return Ok(result);
    }

    // Update employee
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeRequestDto dto)
    {
        var result = await _employeeRepository.UpdateEmployee(id, dto);
        return Ok(result);
    }

    // DELETE employee
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeRepository.DeleteEmployee(id);
        return Ok(result);
    }

    // GET employee leaves
    [HttpGet("leaves/{id}")]
    public async Task<IActionResult> GetEmployeeLeaves(int id)
    {
        var result = await _employeeRepository.GetEmployeeLeaves(id);
        return Ok(result);
    }

    // GET employees on-leave
    [HttpGet("on-leave")]
    public async Task<IActionResult> GetEmployeesOnLeave()
    {
        var result = await _employeeRepository.GetEmployeesCurrentlyOnLeave();
        return Ok(result);
    }
}