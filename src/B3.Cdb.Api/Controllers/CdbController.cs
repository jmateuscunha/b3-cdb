using B3.Cdb.Application.Commands;
using B3.Core;
using B3.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace B3.Cdb.Api.Controllers;


[Route("api/cdb")]
[ExcludeFromCodeCoverage]
public class CdbController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] CalculateCbdDto dto, CancellationToken ct)
    {
        var (result, validation) = await _mediator.Send(new CalculateCdbCommand(dto.Value, dto.Months), ct);
        return CustomResponse(result, validation);
    }    
}