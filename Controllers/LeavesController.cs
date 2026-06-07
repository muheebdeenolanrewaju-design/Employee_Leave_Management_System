using Employee_Leave_Management_System.Models;
using Employee_Leave_Management_System.Models.Dtos;
using Employee_Leave_Management_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Leave_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeavesController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;

    public LeavesController(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetAllLeaves()
    {
        var leaves = await _leaveRepository.GetAllLeaves();

        return Ok(leaves);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveRequest>> GetLeaveById(int id)
    {
        var leave = await _leaveRepository.GetLeaveById(id);

        return Ok(leave);
    }

    [HttpPost]
    public async Task<ActionResult<LeaveRequest>> CreateLeave(
        CreateLeaveRequestDto dto)
    {
        var leave = await _leaveRepository.CreateLeave(dto);

        return CreatedAtAction(
            nameof(GetLeaveById),
            new { id = leave.Id },
            leave);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LeaveRequest>> UpdateLeave(
        int id,
        UpdateLeaveRequestDto dto)
    {
        var leave = await _leaveRepository.UpdateLeave(id, dto);

        return Ok(leave);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<LeaveRequest>> DeleteLeave(int id)
    {
        var leave = await _leaveRepository.DeleteLeave(id);

        return Ok(leave);
    }

    [HttpPut("approve/{id}")]
    public async Task<ActionResult<LeaveRequest>> ApproveLeave(int id)
    {
        var leave = await _leaveRepository.ApproveLeave(id);

        return Ok(leave);
    }

    [HttpPut("reject/{id}")]
    public async Task<ActionResult<LeaveRequest>> RejectLeave(int id)
    {
        var leave = await _leaveRepository.RejectLeave(id);

        return Ok(leave);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetByStatus(
        string status)
    {
        var leaves = await _leaveRepository.GetLeavesByStatus(status);

        return Ok(leaves);
    }

    [HttpGet("current")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetCurrentLeaves()
    {
        var employees = await _leaveRepository.GetEmployeesCurrentlyOnLeave();

        return Ok(employees);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult> GetStatistics()
    {
        var stats = await _leaveRepository.GetDepartmentLeaveStatistics();

        return Ok(stats);
    }
}