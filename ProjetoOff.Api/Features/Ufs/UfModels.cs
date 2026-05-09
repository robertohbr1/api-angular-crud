using FluentValidation;

namespace ProjetoOff.Api.Features.Ufs;

public record CreateUfRequest(string Id, string Nome);
public record UpdateUfRequest(string Nome);
public record UfResponse(string Id, string Nome, DateTime CreatedAt);

public class CreateUfValidator : AbstractValidator<CreateUfRequest>
{
    public CreateUfValidator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(2);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
    }
}
