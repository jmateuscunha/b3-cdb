using B3.Cdb.Application.Commands;
using FluentValidation;

namespace B3.Cdb.Application.Validators;

public class CalculateCdbCommandValidator : AbstractValidator<CalculateCdbCommand>
{
    private readonly int _baseMonth = 1;

    public CalculateCdbCommandValidator()
    {
        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage($"Value field must be a positive number.");

        RuleFor(x => x.Months)
            .GreaterThan(0)
            .WithMessage($"Months field must have min of {_baseMonth} set.");        
    }
}
