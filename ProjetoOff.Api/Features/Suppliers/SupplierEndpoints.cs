using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjetoOff.Api.Infrastructure.Data;

namespace ProjetoOff.Api.Features.Suppliers;

public static class SupplierEndpoints
{
    public static void MapSupplierEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/suppliers").WithTags("Suppliers");

        group.MapGet("/", async (AppDbContext db) => 
            await db.Suppliers.Select(s => ToResponse(s)).ToListAsync());

        group.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
            await db.Suppliers.FindAsync(id) is Supplier s 
                ? Results.Ok(ToResponse(s)) 
                : Results.NotFound());

        group.MapPost("/", async (CreateSupplierRequest request, IValidator<CreateSupplierRequest> validator, AppDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                RazaoSocial = request.RazaoSocial,
                Cnpj = request.Cnpj,
                Address = request.Address,
                Phone = request.Phone,
                Uf = request.Uf,
                Cna = request.Cna
            };

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return Results.Created($"/api/suppliers/{supplier.Id}", ToResponse(supplier));
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateSupplierRequest request, AppDbContext db) =>
        {
            var supplier = await db.Suppliers.FindAsync(id);
            if (supplier is null) return Results.NotFound();

            supplier.Name = request.Name;
            supplier.RazaoSocial = request.RazaoSocial;
            supplier.Cnpj = request.Cnpj;
            supplier.Address = request.Address;
            supplier.Phone = request.Phone;
            supplier.Uf = request.Uf;
            supplier.Cna = request.Cna;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var supplier = await db.Suppliers.FindAsync(id);
            if (supplier is null) return Results.NotFound();

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static SupplierResponse ToResponse(Supplier s) => 
        new(s.Id, s.Name, s.RazaoSocial, s.Cnpj, s.Address, s.Phone, s.Uf, s.Cna, s.CreatedAt);
}
