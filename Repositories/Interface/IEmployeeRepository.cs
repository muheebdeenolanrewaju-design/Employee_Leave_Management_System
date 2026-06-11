
using Employee_Leave_Management_System.Models.Dtos.Requests;
using Employee_Leave_Management_System.Models.Dtos.Responses;

namespace Employee_Leave_Management_System.Repositories.Interface;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllEmployees();

    Task<EmployeeResponseDto> GetEmployeeById(int id);

    Task<EmployeeResponseDto> CreateEmployee(CreateEmployeeRequestDto dto);

    Task<EmployeeResponseDto> UpdateEmployee(int id, UpdateEmployeeRequestDto dto);

    Task<bool> DeleteEmployee(int id);

    Task<IEnumerable<LeaveRequestResponseDto>> GetEmployeeLeaves(int employeeId);

    Task<IEnumerable<EmployeeResponseDto>> GetEmployeesCurrentlyOnLeave();
}
