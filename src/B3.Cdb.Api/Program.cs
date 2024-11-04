using B3.Cdb.Api.Configurations;
using B3.Cdb.Application.Commands.Handlers;
using B3.Cdb.Application.Commands;
using B3.Cdb.Application.Validators;
using B3.Shared;
using FluentValidation;
using MediatR;
using FluentValidation.Results;

var builder = WebApplication.CreateBuilder(args);

var apiconfiguration = BootstrapConfiguration.GetEnvConfiguration(builder.Configuration);

builder.Services.AddSingleton(apiconfiguration);
builder.Services.AddControllerAndMvcSettings();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
builder.Services.AddScoped<IRequestHandler<CalculateCdbCommand, (CalculatedGrossNetCdbDto, ValidationResult)>, CalculateCdbCommandHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CalculateCdbCommandValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();

app.MapControllers();

app.Run();