using Employee_Leave_Management_System.Models.Dtos.Requests;
using FluentValidation;

public class SubmitLeaveRequestValidator
    : AbstractValidator<SubmitLeaveRequestDto>
{
    public SubmitLeaveRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0);

        RuleFor(x => x.LeaveType)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start Date cannot be later than End Date.");
    }
}