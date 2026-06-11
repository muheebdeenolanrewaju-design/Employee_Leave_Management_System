namespace Employee_Leave_Management_System.Models.Dtos.Requests;

public class LeaveActionRequestDto
{
    public int ApproverId { get; set; }
    public string Reason { get; set; }
}