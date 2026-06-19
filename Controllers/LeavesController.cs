using Employee_Leave_Management_System.Models.Dtos.Requests;
using Employee_Leave_Management_System.Repositories.Interface;
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

    // GET all leaves
    [HttpGet]
    public async Task<IActionResult> GetAllLeaves()
    {
        var result = await _leaveRepository.GetAllLeaves();
        return Ok(result);
    }

    // GET leaves by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveById(int id)
    {
        var result = await _leaveRepository.GetLeaveById(id);
        return Ok(result);
    }

    // Create leave request
    [HttpPost]
    public async Task<IActionResult> CreateLeave(SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.CreateLeave(dto);
        return Ok(result);
    }

    // Update leave request
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeave(int id, SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.UpdateLeave(id, dto);
        return Ok(result);
    }

    // Delete leave request
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeave(int id)
    {
        var result = await _leaveRepository.DeleteLeave(id);
        return Ok(result);
    }

    // Approve leave request
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.ApproveLeave(id, dto);
        return Ok(result);
    }

    // Reject leave request
    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.RejectLeave(id, dto);
        return Ok(result);
    }

    // GET leaves by status
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetLeavesByStatus(string status)
    {
        var result = await _leaveRepository.GetLeavesByStatus(status);
        return Ok(result);
    }

    // GET statistics
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _leaveRepository.GetDepartmentLeaveStatistics();
        return Ok(result);
    }
}