using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOff.Api.Infrastructure.Data;

namespace ProjetoOff.Api.Features.Cnaes;

public static class CnaeEndpoints
{
    public static void MapCnaeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cnaes").WithTags("Cnaes");

        group.MapGet("/", async (AppDbContext db) => 
            await db.Cnaes.Select(c => ToResponse(c)).ToListAsync());

        group.MapGet("/{id}", async (string id, AppDbContext db) =>
            await db.Cnaes.FindAsync(id) is Cnae c 
                ? Results.Ok(ToResponse(c)) 
                : Results.NotFound());

        group.MapPost("/", async (CreateCnaeRequest request, IValidator<CreateCnaeRequest> validator, AppDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var existing = await db.Cnaes.FindAsync(request.Id.ToUpper());
            if (existing is not null) return Results.BadRequest("CNAE já existe.");

            var cnae = new Cnae
            {
                Id = request.Id.ToUpper(),
                Nome = request.Nome
            };

            db.Cnaes.Add(cnae);
            await db.SaveChangesAsync();

            return Results.Created($"/api/cnaes/{cnae.Id}", ToResponse(cnae));
        });

        group.MapPut("/{id}", async (string id, UpdateCnaeRequest request, AppDbContext db) =>
        {
            var cnae = await db.Cnaes.FindAsync(id.ToUpper());
            if (cnae is null) return Results.NotFound();

            cnae.Nome = request.Nome;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (string id, AppDbContext db) =>
        {
            var cnae = await db.Cnaes.FindAsync(id.ToUpper());
            if (cnae is null) return Results.NotFound();

            db.Cnaes.Remove(cnae);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static CnaeResponse ToResponse(Cnae c) => 
        new(c.Id, c.Nome, c.CreatedAt);
}
