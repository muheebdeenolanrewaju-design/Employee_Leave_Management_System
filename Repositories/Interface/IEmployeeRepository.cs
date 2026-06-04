using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos;

namespace Employee_Leave_Management_System.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployees();

    Task<Employee> GetEmployeeById(int id);

    Task<Employee> CreateEmployee(CreateEmployeeDto dto);

    Task<Employee> UpdateEmployee(int id, UpdateEmployeeDto dto);

    Task<Employee> DeleteEmployee(int id);

    Task<IEnumerable<LeaveRequest>> GetEmployeeLeaves(int employeeId);
}