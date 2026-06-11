using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos.Requests;
using Employee_Leave_Management_System.Models.Dtos.Responses;
using Employee_Leave_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE EMPLOYEE
    public async Task<EmployeeResponseDto> CreateEmployee(CreateEmployeeRequestDto dto)
    {
        var exists = await _context.Employees
            .AnyAsync(x => x.Email.ToLower() == dto.Email.ToLower());

        if (exists)
            throw new Exception("Employee already exists");

        var employee = new Employee
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Department = dto.Department,
            DateJoined = DateTime.UtcNow
        };

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        return MapToDto(employee);
    }

    // GET ALL EMPLOYEES
    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployees()
    {
        var employees = await _context.Employees.ToListAsync();
        return employees.Select(MapToDto);
    }

    // GET EMPLOYEE BY ID
    public async Task<EmployeeResponseDto> GetEmployeeById(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
            throw new Exception("Employee not found");

        return MapToDto(employee);
    }

    // UPDATE EMPLOYEE
    public async Task<EmployeeResponseDto> UpdateEmployee(int id, UpdateEmployeeRequestDto dto)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
            throw new Exception("Employee not found");

        employee.FullName = dto.FullName;
        employee.Email = dto.Email;
        employee.Department = dto.Department;

        await _context.SaveChangesAsync();

        return MapToDto(employee);
    }

    // DELETE EMPLOYEE
    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
            throw new Exception("Employee not found");

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return true;
    }

    // GET EMPLOYEE LEAVES
    public async Task<IEnumerable<LeaveRequestResponseDto>> GetEmployeeLeaves(int employeeId)
    {
        var leaves = await _context.LeaveRequests
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.LeaveApprovals)
            .ToListAsync();

        return leaves.Select(l => new LeaveRequestResponseDto
        {
            Id = l.Id,
            EmployeeId = l.EmployeeId,
            LeaveType = l.LeaveType,
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            Reason = l.Reason,
            Status = l.Status,
            DateCreated = l.DateCreated,
            Approvals = l.LeaveApprovals.Select(a => new LeaveApprovalResponseDto
            {
                Id = a.Id,
                ApproverId = a.ApproverId,
                Action = a.Action,
                Reason = a.Reason,
                DateActed = a.DateActed
            }).ToList()
        });
    }

    // EMPLOYEES CURRENTLY ON LEAVE
    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesCurrentlyOnLeave()
    {
        var employees = await _context.LeaveRequests
            .Where(x => x.Status == "Approved")
            .Include(x => x.Employee)
            .Select(x => x.Employee)
            .Distinct()
            .ToListAsync();

        return employees.Select(MapToDto);
    }

    // MAPPING FUNCTION
    private static EmployeeResponseDto MapToDto(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Department = employee.Department,
            DateJoined = employee.DateJoined
        };
    }
}