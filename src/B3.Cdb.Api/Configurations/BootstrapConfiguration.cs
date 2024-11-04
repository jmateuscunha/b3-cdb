using B3.Shared;
using Microsoft.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace B3.Cdb.Api.Configurations;

[ExcludeFromCodeCoverage]
public static class BootstrapConfiguration
{
    public static ApiConfiguration GetEnvConfiguration(IConfiguration configuration)
    {
        return new ApiConfiguration()
        {
            CDB_6 = configuration.GetValue<decimal>("CDB_6"),
            CDB_12 = configuration.GetValue<decimal>("CDB_12"),
            CDB_24 = configuration.GetValue<decimal>("CDB_24"),
            CDB_DEFAULT = configuration.GetValue<decimal>("CDB_DEFAULT"),
            CDI = configuration.GetValue<decimal>("CDI"),
            TD = configuration.GetValue<decimal>("TD"),
        };
    }

    public static IServiceCollection AddControllerAndMvcSettings(this IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
        }).AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.AllowTrailingCommas = true;
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddRouting(options => options.LowercaseUrls = true)
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();

        services.AddCors(op =>
        {
            op.AddDefaultPolicy(pol =>
                {
                    pol.WithOrigins("http://localhost:4200");
                    pol.WithHeaders(HeaderNames.Accept, HeaderNames.ContentType, HeaderNames.Referer, HeaderNames.UserAgent);
                    pol.WithMethods("POST");
                }
            );
        });

        return services;
    }
}
