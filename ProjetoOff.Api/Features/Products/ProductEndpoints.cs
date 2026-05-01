using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOff.Api.Infrastructure.Data;

namespace ProjetoOff.Api.Features.Products;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (AppDbContext db) => 
            await db.Products.Select(p => ToResponse(p)).ToListAsync());

        group.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
            await db.Products.FindAsync(id) is Product p 
                ? Results.Ok(ToResponse(p)) 
                : Results.NotFound());

        group.MapPost("/", async (CreateProductRequest request, IValidator<CreateProductRequest> validator, AppDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Results.Created($"/api/products/{product.Id}", ToResponse(product));
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, AppDbContext db) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product is null) return Results.NotFound();

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product is null) return Results.NotFound();

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static ProductResponse ToResponse(Product p) => 
        new(p.Id, p.Name, p.Description, p.Price, p.CreatedAt);
}
