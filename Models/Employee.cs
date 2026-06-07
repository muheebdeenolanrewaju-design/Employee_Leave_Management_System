using System.Runtime.InteropServices.JavaScript;

namespace Employee_Leave_Management_System.Models;

public class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Department { get; set; }

    public DateTime DateJoined { get; set; } = DateTime.Now;

    public ICollection<LeaveRequest> LeaveRequests { get; set; }
}