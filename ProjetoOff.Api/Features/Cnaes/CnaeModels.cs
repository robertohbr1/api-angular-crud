using FluentValidation;

namespace ProjetoOff.Api.Features.Cnaes;

public record CreateCnaeRequest(string Id, string Nome);
public record UpdateCnaeRequest(string Nome);
public record CnaeResponse(string Id, string Nome, DateTime CreatedAt);

public class CreateCnaeValidator : AbstractValidator<CreateCnaeRequest>
{
    public CreateCnaeValidator()
    {
        RuleFor(x => x.Id).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
    }
}
