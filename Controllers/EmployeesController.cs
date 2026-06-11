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

    // GET: api/employees
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var result = await _employeeRepository.GetAllEmployees();
        return Ok(result);
    }

    // GET: api/employees/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var result = await _employeeRepository.GetEmployeeById(id);
        return Ok(result);
    }

    // POST: api/employees
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequestDto dto)
    {
        var result = await _employeeRepository.CreateEmployee(dto);
        return Ok(result);
    }

    // PUT: api/employees/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeRequestDto dto)
    {
        var result = await _employeeRepository.UpdateEmployee(id, dto);
        return Ok(result);
    }

    // DELETE: api/employees/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeRepository.DeleteEmployee(id);
        return Ok(result);
    }

    // GET: api/employees/{id}/leaves
    [HttpGet("{id}/leaves")]
    public async Task<IActionResult> GetEmployeeLeaves(int id)
    {
        var result = await _employeeRepository.GetEmployeeLeaves(id);
        return Ok(result);
    }

    // GET: api/employees/on-leave
    [HttpGet("on-leave")]
    public async Task<IActionResult> GetEmployeesOnLeave()
    {
        var result = await _employeeRepository.GetEmployeesCurrentlyOnLeave();
        return Ok(result);
    }
}