using Employee_Leave_Management_System.Helper;
using Employee_Leave_Management_System.Models.Dtos.Requests;
using FluentValidation;

public class CreateEmployeeRequestValidator
    : AbstractValidator<CreateEmployeeRequestDto>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

       RuleFor(x => x.Department)
           .Must(d => Departments.All.Contains(d))
           .WithMessage("Invalid department");
    }
}