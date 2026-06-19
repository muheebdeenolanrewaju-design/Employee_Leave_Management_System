using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Enums;
using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos.Requests;
using Employee_Leave_Management_System.Models.Dtos.Responses;
using Employee_Leave_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories.Implementations;

public class LeaveRepository : ILeaveRepository
{
    private readonly ApplicationDbContext _context;

    public LeaveRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE LEAVE REQUEST
    public async Task<LeaveRequestResponseDto> CreateLeave(SubmitLeaveRequestDto dto)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(x => x.Id == dto.EmployeeId);
     
        var  today = DateTime.Now;
        
        if (dto.EndDate<today || dto.StartDate < today || dto.EndDate < dto.StartDate )
        {
            throw new Exception("Invalid leave date");
        }

        if (!employeeExists)
            throw new Exception("Employee does not exist");

        if (!Enum.TryParse<LeaveType>(dto.LeaveType, true, out var leaveType))
        {
            throw new Exception("Invalid leave type");
        }
        
        // check for overlapping
        var overlappingLeave = await _context.LeaveRequests
            .AnyAsync(x =>
                x.EmployeeId == dto.EmployeeId &&
                dto.StartDate <= x.EndDate &&
                dto.EndDate >= x.StartDate &&
                x.Status != LeaveStatus.Rejected);

        if (overlappingLeave)
        {
            throw new Exception("Employee already has a leave request during this period");
        }

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = leaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            Status = LeaveStatus.Pending,
            DateCreated = DateTime.UtcNow
        };

        await _context.LeaveRequests.AddAsync(leave);
        await _context.SaveChangesAsync();

        return await GetLeaveById(leave.Id);
    }

    // GET ALL LEAVES
    public async Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaves()
    {
        var leaves = await _context.LeaveRequests
            //.Include(x => x.LeaveApprovals)
            .ToListAsync();

        return leaves.Select(MapToDto);
    }

    // GET LEAVE BY ID
    public async Task<LeaveRequestResponseDto> GetLeaveById(int id)
    {
        var leave = await _context.LeaveRequests
            .Include(x => x.LeaveApprovals)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
            throw new Exception("Leave request not found");

        return MapToDto(leave);
    }

    // UPDATE LEAVE
    public async Task<LeaveRequestResponseDto> UpdateLeave(int id, SubmitLeaveRequestDto dto)
    {
        var leave = await _context.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        var  today = DateTime.Now;
        
        if (dto.EndDate<today || dto.StartDate < today || dto.EndDate < dto.StartDate)
        {
            throw new Exception("Invalid leave date");
        }
        
        if (leave == null)
            throw new Exception("Leave request not found");

        if (!Enum.TryParse<LeaveType>(dto.LeaveType, true, out var leaveType))
        {
            throw new Exception("Invalid leave type");
        }
        
        var overlappingLeave = await _context.LeaveRequests
            .AnyAsync(x =>
                x.EmployeeId == dto.EmployeeId &&
                dto.StartDate <= x.EndDate &&
                dto.EndDate >= x.StartDate &&
                x.Status != LeaveStatus.Rejected);

        if (overlappingLeave)
        {
            throw new Exception("Employee already has a leave request during this period");
        }

        leave.LeaveType = leaveType;
        leave.StartDate = dto.StartDate;
        leave.EndDate = dto.EndDate;
        leave.Reason = dto.Reason;

        await _context.SaveChangesAsync();

        return await GetLeaveById(id);
    }

    // DELETE LEAVE
    public async Task<bool> DeleteLeave(int id)
    {
        var leave = await _context.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
            throw new Exception("Leave request not found");

        _context.LeaveRequests.Remove(leave);
        await _context.SaveChangesAsync();

        return true;
    }

    // APPROVE LEAVE (FIRST OR SECOND APPROVAL)
    public async Task<LeaveRequestResponseDto> ApproveLeave(int leaveId, LeaveActionRequestDto dto)
    {
        var leave = await _context.LeaveRequests
            .Include(x => x.LeaveApprovals)
            .FirstOrDefaultAsync(x => x.Id == leaveId);

        if (leave == null)
            throw new Exception("Leave not found");

        if (leave.EmployeeId == dto.ApproverId)
            throw new Exception("Employee cannot approve their own leave");

        var alreadyActed = leave.LeaveApprovals
            .Any(x => x.ApproverId == dto.ApproverId);

        if (alreadyActed)
            throw new Exception("Approver already acted on this request");

        var approval = new LeaveApproval
        {
            LeaveRequestId = leaveId,
            ApproverId = dto.ApproverId,
            Action = "Approve",
            Reason = dto.Reason,
            DateActed = DateTime.UtcNow
        };

        await _context.LeaveApprovals.AddAsync(approval);

        // STATE MACHINE LOGIC
        if (leave.Status == LeaveStatus.Pending)
            leave.Status = LeaveStatus.Processing;
        else if (leave.Status == LeaveStatus.Processing)
            leave.Status = LeaveStatus.Approved;

        await _context.SaveChangesAsync();

        return await GetLeaveById(leaveId);
    }

    // REJECT LEAVE
    public async Task<LeaveRequestResponseDto> RejectLeave(int leaveId, LeaveActionRequestDto dto)
    {
        var leave = await _context.LeaveRequests
            .Include(x => x.LeaveApprovals)
            .FirstOrDefaultAsync(x => x.Id == leaveId);

        if (leave == null)
            throw new Exception("Leave not found");

        if (leave.EmployeeId == dto.ApproverId)
            throw new Exception("Employee cannot reject their own leave");

        var alreadyActed = leave.LeaveApprovals
            .Any(x => x.ApproverId == dto.ApproverId);

        if (alreadyActed)
            throw new Exception("Approver already acted on this request");

        var approval = new LeaveApproval
        {
            LeaveRequestId = leaveId,
            ApproverId = dto.ApproverId,
            Action = "Reject",
            Reason = dto.Reason,
            DateActed = DateTime.UtcNow
        };

        await _context.LeaveApprovals.AddAsync(approval);

        leave.Status = LeaveStatus.Rejected;

        await _context.SaveChangesAsync();

        return await GetLeaveById(leaveId);
    }

    // FILTER BY STATUS
    public async Task<IEnumerable<LeaveRequestResponseDto>>  GetLeavesByStatus(string status)
    {
        if (!Enum.TryParse<LeaveStatus>(status, true, out var parsedStatus))
            throw new Exception("Invalid status");

        var leaves = await _context.LeaveRequests
            .Where(x => x.Status == parsedStatus)
            .Include(x => x.LeaveApprovals)
            .ToListAsync();

        return leaves.Select(MapToDto);
    }

    // STATISTICS
    public async Task<object> GetDepartmentLeaveStatistics()
    {
        var stats = await _context.LeaveRequests
            .Include(x => x.Employee)
            .GroupBy(x => x.Employee.Department)
            .Select(g => new
            {
                Department = g.Key,
                TotalLeaves = g.Count()
            })
            .ToListAsync();

        return stats;
    }

    // MAPPING FUNCTION
    private static LeaveRequestResponseDto MapToDto(LeaveRequest l)
    {
        return new LeaveRequestResponseDto
        {
            Id = l.Id,
            EmployeeId = l.EmployeeId,
            LeaveType = l.LeaveType.ToString(),
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            Reason = l.Reason,
            Status = l.Status.ToString(),
            DateCreated = l.DateCreated,
            Approvals = l.LeaveApprovals?.Select(a => new LeaveApprovalResponseDto
            {
                Id = a.Id,
                ApproverId = a.ApproverId,
                Action = a.Action,
                Reason = a.Reason,
                DateActed = a.DateActed
            }).ToList()
        };
    }
}