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

    // GET: api/leaves
    [HttpGet]
    public async Task<IActionResult> GetAllLeaves()
    {
        var result = await _leaveRepository.GetAllLeaves();
        return Ok(result);
    }

    // GET: api/leaves/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveById(int id)
    {
        var result = await _leaveRepository.GetLeaveById(id);
        return Ok(result);
    }

    // POST: api/leaves
    [HttpPost]
    public async Task<IActionResult> CreateLeave(SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.CreateLeave(dto);
        return Ok(result);
    }

    // PUT: api/leaves/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeave(int id, SubmitLeaveRequestDto dto)
    {
        var result = await _leaveRepository.UpdateLeave(id, dto);
        return Ok(result);
    }

    // DELETE: api/leaves/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeave(int id)
    {
        var result = await _leaveRepository.DeleteLeave(id);
        return Ok(result);
    }

    // POST: api/leaves/{id}/approve
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.ApproveLeave(id, dto);
        return Ok(result);
    }

    // POST: api/leaves/{id}/reject
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> RejectLeave(int id, LeaveActionRequestDto dto)
    {
        var result = await _leaveRepository.RejectLeave(id, dto);
        return Ok(result);
    }

    // GET: api/leaves/status/{status}
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetLeavesByStatus(string status)
    {
        var result = await _leaveRepository.GetLeavesByStatus(status);
        return Ok(result);
    }

    // GET: api/leaves/statistics
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _leaveRepository.GetDepartmentLeaveStatistics();
        return Ok(result);
    }
}