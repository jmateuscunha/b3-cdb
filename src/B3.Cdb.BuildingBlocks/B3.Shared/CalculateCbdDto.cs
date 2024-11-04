using System.ComponentModel.DataAnnotations;

namespace B3.Shared;

public record CalculateCbdDto([Required] decimal Value, [Required] int Months);
