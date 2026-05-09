using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using ProjetoOff.Api.Infrastructure.Data;
using ProjetoOff.Api.Infrastructure.Security;
using ProjetoOff.Api.Features.Products;
using ProjetoOff.Api.Features.Clients;
using ProjetoOff.Api.Features.Suppliers;
using ProjetoOff.Api.Features.Ufs;
using ProjetoOff.Api.Features.Cnaes;

var builder = WebApplication.CreateBuilder(args);

// 1. Infrastructure Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProjetoOffDb"));

builder.Services.AddScoped<ISecurityService, SecurityService>();

// 2. CORS
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 3. Security (JWT)
var secret = builder.Configuration["Jwt:Secret"] ?? "a-very-long-and-secure-secret-key-1234567890";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ProjetoOff",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ProjetoOff",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddAuthorization();

// 4. API Docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// 6. Feature Endpoints
app.MapProductEndpoints();
app.MapClientEndpoints();
app.MapSupplierEndpoints();
app.MapUfEndpoints();
app.MapCnaeEndpoints();

// Auth endpoint for testing
app.MapPost("/api/auth/login", (string username, ISecurityService security) => 
    Results.Ok(new { token = security.GenerateJwtToken(username) }))
    .WithTags("Auth");

app.Run();

public partial class Program { }
