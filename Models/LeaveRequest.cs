namespace Employee_Leave_Management_System.Models;

public class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; }

    public DateTime StartDate { get; set; } 

    public DateTime EndDate { get; set; }

    public string Reason { get; set; }

    public string Status { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public Employee Employee { get; set; }
}