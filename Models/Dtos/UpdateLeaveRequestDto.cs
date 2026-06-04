namespace Employee_Leave_Management_System.Models.Dtos;

public class UpdateLeaveRequestDto
{
    public string LeaveType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Reason { get; set; }
}