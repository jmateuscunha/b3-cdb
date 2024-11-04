using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace B3.Core;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult CustomResponse(object result = null, ValidationResult validation = null)
    {
        if (validation.IsValid == false)
        {
            return UnprocessableEntity(new
            {
                Timestamp = DateTime.UtcNow,
                Errors = validation.Errors.Select(x => x.ErrorMessage)
            });
        }

        return Ok(result);
    }
}