using Employee_Leave_Management_System.Models.Dtos.Requests;
using FluentValidation;

namespace Employee_Leave_Management_System.Validators;

public class LeaveActionRequestValidator
    : AbstractValidator<LeaveActionRequestDto>
{
    public LeaveActionRequestValidator()
    {
        RuleFor(x => x.ApproverId)
            .GreaterThan(0);
    }
}