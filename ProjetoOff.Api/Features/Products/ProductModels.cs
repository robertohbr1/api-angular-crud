using FluentValidation;

namespace ProjetoOff.Api.Features.Products;

public record CreateProductRequest(string Name, string Description, decimal Price);
public record UpdateProductRequest(string Name, string Description, decimal Price);
public record ProductResponse(Guid Id, string Name, string Description, decimal Price, DateTime CreatedAt);

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
