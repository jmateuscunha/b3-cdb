using B3.Shared;
using FluentValidation.Results;
using MediatR;

namespace B3.Cdb.Application.Commands;

public class CalculateCdbCommand(decimal value, int months) : IRequest<(CalculatedGrossNetCdbDto, ValidationResult)>
{
    public int Months { get; set; } = months;
    public decimal Value { get; set; } = value;
}
