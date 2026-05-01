# ProjetoOff API - AI Maintenance Guide

## Architecture
This project uses **Vertical Slice Architecture**. Each feature (CRUD) lives in its own folder under `Features/`.

## How to add a new entity (e.g., "Category")
1. Create a folder `Features/Categories/`.
2. Create `Category.cs` (Domain model).
3. Create `CategoryModels.cs` (DTOs and FluentValidation).
4. Create `CategoryEndpoints.cs` (Minimal API routes).
5. Add the `DbSet<Category>` to `Infrastructure/Data/AppDbContext.cs`.
6. Register endpoints in `Program.cs` using `app.MapCategoryEndpoints();`.

## Code Style
- **Minimal APIs**: Prefer Minimal APIs over Controllers for simplicity and performance.
- **FluentValidation**: Always validate requests using FluentValidation.
- **Dependency Injection**: Use constructor injection or parameter injection in Minimal API handlers.
- **Security**: All endpoints should consider authentication if needed (add `.RequireAuthorization()`).

## Commands
- Build: `dotnet build`
- Run: `dotnet run --project ProjetoOff.Api`
- Test: `dotnet test`
- Add Package: `dotnet add package <PackageName>`
