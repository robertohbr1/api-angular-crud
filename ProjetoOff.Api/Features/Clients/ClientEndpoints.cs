using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjetoOff.Api.Infrastructure.Data;

namespace ProjetoOff.Api.Features.Clients;

public static class ClientEndpoints
{
    public static void MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clients").WithTags("Clients");

        group.MapGet("/", async (AppDbContext db) => 
            await db.Clients.Select(c => ToResponse(c)).ToListAsync());

        group.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
            await db.Clients.FindAsync(id) is Client c 
                ? Results.Ok(ToResponse(c)) 
                : Results.NotFound());

        group.MapPost("/", async (CreateClientRequest request, IValidator<CreateClientRequest> validator, AppDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Cnpj = request.Cnpj,
                Address = request.Address,
                Phone = request.Phone,
                Uf = request.Uf,
                Cnae = request.Cnae
            };

            db.Clients.Add(client);
            await db.SaveChangesAsync();

            return Results.Created($"/api/clients/{client.Id}", ToResponse(client));
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateClientRequest request, AppDbContext db) =>
        {
            var client = await db.Clients.FindAsync(id);
            if (client is null) return Results.NotFound();

            client.Name = request.Name;
            client.Cnpj = request.Cnpj;
            client.Address = request.Address;
            client.Phone = request.Phone;
            client.Uf = request.Uf;
            client.Cnae = request.Cnae;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var client = await db.Clients.FindAsync(id);
            if (client is null) return Results.NotFound();

            db.Clients.Remove(client);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static ClientResponse ToResponse(Client c) => 
        new(c.Id, c.Name, c.Cnpj, c.Address, c.Phone, c.Uf, c.Cnae, c.CreatedAt);
}
