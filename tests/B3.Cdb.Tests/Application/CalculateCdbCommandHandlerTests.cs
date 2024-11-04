using B3.Cdb.Application.Commands;
using B3.Cdb.Application.Commands.Handlers;
using B3.Shared;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace B3.Cdb.Tests.Application;

public class CalculateCdbCommandHandlerTests
{
    private readonly Mock<IValidator<CalculateCdbCommand>> _mockValidator;
    private readonly ApiConfiguration _configuration;
    private readonly Faker _faker;

    public CalculateCdbCommandHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CalculateCdbCommand>>();

        // Configurando valores fictícios para o ApiConfiguration
        _configuration = new ApiConfiguration
        {
            CDI = 0.1m,
            TD = 0.01m,
            CDB_6 = 0.225m,
            CDB_12 = 0.2m,
            CDB_24 = 0.175m,
            CDB_DEFAULT = 0.15m
        };

        _faker = new Faker();
    }

    [Fact]
    [Trait("CDB", "InvalidCommand")]
    public async Task Handle_InvalidCommand_ReturnsValidationErrors()
    {
        // Arrange
        var command = new CalculateCdbCommand(_faker.Random.Decimal(1000, 10000), _faker.Random.Int(1, 36));

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Value", "Invalid Value")
        });

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var handler = new CalculateCdbCommandHandler(_mockValidator.Object, _configuration);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Item2.IsValid);
        Assert.NotNull(result.Item2.Errors);
        Assert.Equal("Invalid Value", result.Item2.Errors[0].ErrorMessage);
    }

    [Fact]
    [Trait("CDB", "ValidCommand")]
    public async Task Handle_ValidCommand_ReturnsCorrectGrossAndNetValues()
    {
        // Arrange
        var command = new CalculateCdbCommand (1000m, 12);

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var handler = new CalculateCdbCommandHandler(_mockValidator.Object, _configuration);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var expectedGross = 1000m * (decimal)Math.Pow((double)(1 + (_configuration.CDI * _configuration.TD)), 12) - 1000m;
        var expectedTax = expectedGross * _configuration.CDB_12;
        var expectedNet = expectedGross - expectedTax;

        Assert.Equal(decimal.Truncate((expectedGross + 1000m) * 100m) / 100m, result.Item1.Gross);
        Assert.Equal(decimal.Truncate((expectedNet + 1000m) * 100m) / 100m, result.Item1.Net);
        Assert.True(result.Item2.IsValid);
    }

    [Theory]
    [InlineData(5, 0.225)] // Até 6 meses: 22.5%
    [InlineData(10, 0.2)]  // Até 12 meses: 20%
    [InlineData(18, 0.175)] // Até 24 meses: 17.5%
    [InlineData(30, 0.15)] // Acima de 24 meses: 15%
    [Trait("CDB", "CorrectTaxRate")]
    public async Task Handle_CorrectTaxRateBasedOnMonths(int months, decimal expectedTaxRate)
    {
        // Arrange
        var command = new CalculateCdbCommand ( 1000m, months );

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var handler = new CalculateCdbCommandHandler(_mockValidator.Object, _configuration);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var expectedGross = 1000m * (decimal)Math.Pow((double)(1 + (_configuration.CDI * _configuration.TD)), months) - 1000m;
        var expectedTax = expectedGross * expectedTaxRate;
        var expectedNet = expectedGross - expectedTax;

        Assert.Equal(decimal.Truncate((expectedGross + 1000m) * 100m) / 100m, result.Item1.Gross);
        Assert.Equal(decimal.Truncate((expectedNet + 1000m) * 100m) / 100m, result.Item1.Net);
        Assert.True(result.Item2.IsValid);
    }
}
