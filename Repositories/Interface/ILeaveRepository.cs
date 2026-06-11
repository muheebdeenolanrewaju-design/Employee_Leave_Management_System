

using Employee_Leave_Management_System.Models.Dtos.Requests;
using Employee_Leave_Management_System.Models.Dtos.Responses;

namespace Employee_Leave_Management_System.Repositories.Interface;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaves();

    Task<LeaveRequestResponseDto> GetLeaveById(int id);

    Task<LeaveRequestResponseDto> CreateLeave(SubmitLeaveRequestDto dto);

    Task<LeaveRequestResponseDto> UpdateLeave(int id, SubmitLeaveRequestDto dto);

    Task<bool> DeleteLeave(int id);

    Task<LeaveRequestResponseDto> ApproveLeave(int leaveId, LeaveActionRequestDto dto);

    Task<LeaveRequestResponseDto> RejectLeave(int leaveId, LeaveActionRequestDto dto);

    Task<IEnumerable<LeaveRequestResponseDto>> GetLeavesByStatus(string status);

    Task<object> GetDepartmentLeaveStatistics();
}