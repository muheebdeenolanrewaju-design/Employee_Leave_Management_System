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
            .NotEmpty();
    }
}