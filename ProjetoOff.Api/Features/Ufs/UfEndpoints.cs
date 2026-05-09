using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOff.Api.Infrastructure.Data;

namespace ProjetoOff.Api.Features.Ufs;

public static class UfEndpoints
{
    public static void MapUfEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ufs").WithTags("Ufs");

        group.MapGet("/", async (AppDbContext db) => 
            await db.Ufs.Select(u => ToResponse(u)).ToListAsync());

        group.MapGet("/{id}", async (string id, AppDbContext db) =>
            await db.Ufs.FindAsync(id) is Uf u 
                ? Results.Ok(ToResponse(u)) 
                : Results.NotFound());

        group.MapPost("/", async (CreateUfRequest request, IValidator<CreateUfRequest> validator, AppDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var existing = await db.Ufs.FindAsync(request.Id.ToUpper());
            if (existing is not null) return Results.BadRequest("UF já existe.");

            var uf = new Uf
            {
                Id = request.Id.ToUpper(),
                Nome = request.Nome
            };

            db.Ufs.Add(uf);
            await db.SaveChangesAsync();

            return Results.Created($"/api/ufs/{uf.Id}", ToResponse(uf));
        });

        group.MapPut("/{id}", async (string id, UpdateUfRequest request, AppDbContext db) =>
        {
            var uf = await db.Ufs.FindAsync(id.ToUpper());
            if (uf is null) return Results.NotFound();

            uf.Nome = request.Nome;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (string id, AppDbContext db) =>
        {
            var uf = await db.Ufs.FindAsync(id.ToUpper());
            if (uf is null) return Results.NotFound();

            db.Ufs.Remove(uf);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static UfResponse ToResponse(Uf u) => 
        new(u.Id, u.Nome, u.CreatedAt);
}
