namespace Employee_Leave_Management_System.Models.Dtos.Requests;

public class CreateEmployeeRequestDto
{
    public string FullName { get; set; }

    public string Email { get; set; }

    public string Department { get; set; }
}