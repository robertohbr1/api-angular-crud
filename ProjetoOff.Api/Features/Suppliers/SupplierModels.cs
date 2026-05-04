using FluentValidation;

namespace ProjetoOff.Api.Features.Suppliers;

public record CreateSupplierRequest(string Name, string RazaoSocial, string Cnpj, string Address, string Phone, string Uf, string Cna);
public record UpdateSupplierRequest(string Name, string RazaoSocial, string Cnpj, string Address, string Phone, string Uf, string Cna);
public record SupplierResponse(Guid Id, string Name, string RazaoSocial, string Cnpj, string Address, string Phone, string Uf, string Cna, DateTime CreatedAt);

public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cnpj).NotEmpty().Length(14); // Assuming raw numbers
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Uf).NotEmpty().Length(2);
        RuleFor(x => x.Cna).NotEmpty();
    }
}
