using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories.Implementation;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get All Employees
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await _dbContext.Employees
            .ToListAsync();
    }

    // Get Employee By Id
    public async Task<Employee> GetEmployeeById(int id)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        return employee;
    }

    // Create Employee
    public async Task<Employee> CreateEmployee(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Department = dto.Department,
            DateJoined = dto.DateJoined
        };

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        return employee;
    }

    // Update Employee
    public async Task<Employee> UpdateEmployee(int id, UpdateEmployeeDto dto)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        employee.FullName = dto.FullName;
        employee.Email = dto.Email;
        employee.Department = dto.Department;

        await _dbContext.SaveChangesAsync();

        return employee;
    }

    // Delete Employee
    public async Task<Employee> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        _dbContext.Employees.Remove(employee);

        await _dbContext.SaveChangesAsync();

        return employee;
    }

    // Get Employee Leave History
    public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeaves(int employeeId)
    {
        var employee = await _dbContext.Employees
            .Include(e => e.LeaveRequests)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        return employee.LeaveRequests;
    }
}