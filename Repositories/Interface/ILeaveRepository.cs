using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos;

namespace Employee_Leave_Management_System.Repositories;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequest>> GetAllLeaves();

    Task<LeaveRequest> GetLeaveById(int id);

    Task<LeaveRequest> CreateLeave(CreateLeaveRequestDto dto);

    Task<LeaveRequest> UpdateLeave(int id, UpdateLeaveRequestDto dto);

    Task<LeaveRequest> DeleteLeave(int id);

    Task<LeaveRequest> ApproveLeave(int id);

    Task<LeaveRequest> RejectLeave(int id);

    Task<IEnumerable<LeaveRequest>> GetLeavesByStatus(string status);

    Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave();

    Task<object> GetDepartmentLeaveStatistics();
}