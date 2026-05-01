# Walkthrough: Secure & AI-Friendly C# API

I have successfully built a robust C# API project optimized for security and AI maintenance.

## Accomplishments

### 1. Architectural Structure
- **Vertical Slice Architecture**: All logic for a feature (Products) is contained within `Features/Products/`. This makes it extremely easy for an AI to modify or extend.
- **Minimal APIs**: Used for a clean, low-boilerplate code base.

### 2. Security Features
- **JWT Authentication**: Implemented a `SecurityService` for token generation and configured `Program.cs` for validation.
- **Input Validation**: Integrated **FluentValidation** to ensure data integrity.

### 3. Database
- **EF Core (In-Memory)**: Configured for fast testing and easy swapping to production databases.

### 4. AI Maintenance
- **CLAUDE.md**: Created a dedicated guide for AI agents on how to maintain and extend this specific project.

## Project Structure
- `ProjetoOff.Api/`: Main API project.
  - `Features/`: Feature slices (e.g., Products).
  - `Infrastructure/`: Global services and DB context.
- `ProjetoOff.Tests/`: Integration tests using xUnit and FluentAssertions.

## Verification Results
- `dotnet build`: Successful.
- `dotnet test`: 3 tests passed (Integration tests for Product CRUD).

## How to run
1. Go to `c:\Projetos\ProjetoOff\api`.
2. Run `dotnet run --project ProjetoOff.Api`.
3. Open `https://localhost:7xxx/swagger` to interact with the API.
