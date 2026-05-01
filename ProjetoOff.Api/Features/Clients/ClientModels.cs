using FluentValidation;

namespace ProjetoOff.Api.Features.Clients;

public record CreateClientRequest(string Name, string Cnpj, string Address, string Phone);
public record UpdateClientRequest(string Name, string Cnpj, string Address, string Phone);
public record ClientResponse(Guid Id, string Name, string Cnpj, string Address, string Phone, DateTime CreatedAt);

public class CreateClientValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cnpj).NotEmpty().Length(14); // Assuming raw numbers
        RuleFor(x => x.Phone).NotEmpty();
    }
}
