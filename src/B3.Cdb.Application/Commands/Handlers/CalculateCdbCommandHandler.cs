using B3.Shared;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace B3.Cdb.Application.Commands.Handlers;

public class CalculateCdbCommandHandler(IValidator<CalculateCdbCommand> validator, ApiConfiguration configuration) : IRequestHandler<CalculateCdbCommand, (CalculatedGrossNetCdbDto, ValidationResult)>
{
    private readonly IValidator<CalculateCdbCommand> _validator = validator;
    private readonly ApiConfiguration _configuration = configuration;

    public async Task<(CalculatedGrossNetCdbDto, ValidationResult)> Handle(CalculateCdbCommand request, CancellationToken ct)
    {
        var validateRules = await _validator.ValidateAsync(request, ct);

        if (validateRules.IsValid == false)
            return (new CalculatedGrossNetCdbDto(default, default), validateRules);

        decimal gross = request.Value;

        for (int i = 1; i <= request.Months; i++)
            gross *= (1 + (_configuration.CDI * _configuration.TD));

        decimal grossValue = gross - request.Value;
        decimal taxValue = GetTaxBasedOnMonths(grossValue, request.Months);
        decimal netValueAfterTaxes = grossValue - taxValue;

        return (new CalculatedGrossNetCdbDto(decimal.Truncate((grossValue + request.Value) * 100m) / 100m,
                                             decimal.Truncate((netValueAfterTaxes + request.Value) * 100m) / 100m), validateRules);
    }

    private decimal GetTaxBasedOnMonths(decimal grossValue, int months)
    {
        return months switch
        {
            <= 6 => grossValue * _configuration.CDB_6,
            <= 12 => grossValue * _configuration.CDB_12,
            <= 24 => grossValue * _configuration.CDB_24,
            _ => grossValue * _configuration.CDB_DEFAULT,
        };
    }
}
