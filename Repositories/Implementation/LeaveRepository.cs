using Employee_Leave_Management_System.Data;
using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Employee_Leave_Management_System.Repositories.Implementation;

public class LeaveRepository : ILeaveRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get All Leave Requests
    public async Task<IEnumerable<LeaveRequest>> GetAllLeaves()
    {
        return await _dbContext.LeaveRequests
            .Include(l => l.Employee)
            .ToListAsync();
    }

    // Get Leave By Id
    public async Task<LeaveRequest> GetLeaveById(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .Include(l => l.Employee)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leave == null)
            throw new KeyNotFoundException("Leave request not found");

        return leave;
    }

    // Create Leave Request
    public async Task<LeaveRequest> CreateLeave(CreateLeaveRequestDto dto)
    {
        // Employee must exist
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == dto.EmployeeId);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        // StartDate cannot be after EndDate
        if (dto.StartDate > dto.EndDate)
            throw new Exception("Start Date cannot be later than End Date");

        // Check overlapping leave
        bool overlap = await _dbContext.LeaveRequests
            .AnyAsync(l =>
                l.EmployeeId == dto.EmployeeId &&
                dto.StartDate <= l.EndDate &&
                dto.EndDate >= l.StartDate);

        if (overlap)
            throw new Exception("Employee already has a leave request during this period");

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            Status = "Pending",
            DateCreated = DateTime.Now
        };

        await _dbContext.LeaveRequests.AddAsync(leave);
        await _dbContext.SaveChangesAsync();

        return leave;
    }

    // Update Leave
    public async Task<LeaveRequest> UpdateLeave(int id, UpdateLeaveRequestDto dto)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leave == null)
            throw new KeyNotFoundException("Leave request not found");

        if (dto.StartDate > dto.EndDate)
            throw new Exception("Start Date cannot be later than End Date");

        leave.LeaveType = dto.LeaveType;
        leave.StartDate = dto.StartDate;
        leave.EndDate = dto.EndDate;
        leave.Reason = dto.Reason;

        await _dbContext.SaveChangesAsync();

        return leave;
    }

    // Delete Leave
    public async Task<LeaveRequest> DeleteLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leave == null)
            throw new KeyNotFoundException("Leave request not found");

        _dbContext.LeaveRequests.Remove(leave);

        await _dbContext.SaveChangesAsync();

        return leave;
    }

    // Approve Leave
    public async Task<LeaveRequest> ApproveLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leave == null)
            throw new KeyNotFoundException("Leave request not found");

        leave.Status = "Approved";

        await _dbContext.SaveChangesAsync();

        return leave;
    }

    // Reject Leave
    public async Task<LeaveRequest> RejectLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leave == null)
            throw new KeyNotFoundException("Leave request not found");

        leave.Status = "Rejected";

        await _dbContext.SaveChangesAsync();

        return leave;
    }

    // Filter By Status
    public async Task<IEnumerable<LeaveRequest>> GetLeavesByStatus(string status)
    {
        return await _dbContext.LeaveRequests
            .Where(l => l.Status == status)
            .ToListAsync();
    }

    // Employees Currently On Leave
    public async Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave()
    {
        DateTime today = DateTime.Today;

        return await _dbContext.Employees
            .Where(e => e.LeaveRequests.Any(l =>
                l.Status == "Approved" &&
                l.StartDate <= today &&
                l.EndDate >= today))
            .ToListAsync();
    }

    // Department Statistics
    public async Task<object> GetDepartmentLeaveStatistics()
    {
        var stats = await _dbContext.LeaveRequests
            .Include(l => l.Employee)
            .GroupBy(l => l.Employee.Department)
            .Select(g => new
            {
                Department = g.Key,
                TotalRequests = g.Count()
            })
            .ToListAsync();

        return stats;
    }
}