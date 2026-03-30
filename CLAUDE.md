# CLAUDE.md

## Overview

Official Kontent.ai Management SDK for .NET. Wraps the Kontent.ai Management API with typed models and a fluent client interface.

## Development Commands

```bash
# Build
dotnet build Kontent.Ai.Management.sln

# Run all tests
dotnet test Kontent.Ai.Management.sln

# Run specific test project
dotnet test Kontent.Ai.Management.Tests/Kontent.Ai.Management.Tests.csproj
```

## Project Structure

- `Kontent.Ai.Management/` - Main SDK library
  - `ManagementClient.cs` - Core client, partial classes split by domain (`ManagementClient.*.cs`)
  - `IManagementClient.cs` - Client interface
  - `Models/` - Request/response models organized by domain
  - `Modules/UrlBuilder/` - URL construction for API endpoints
- `Kontent.Ai.Management.Tests/` - Unit tests
  - `ManagementClientTests/` - Tests per domain area
  - `Data/` - Fake JSON response fixtures
  - `Base/FileSystemFixture.cs` - Test helper that creates mock clients from JSON fixtures
- `Kontent.Ai.Management.Helpers/` - Helper utilities
- `Kontent.Ai.Management.Helpers.Tests/` - Helper tests

## Key Patterns

- **Partial client classes**: `ManagementClient` is split into partial classes per domain (e.g. `ManagementClient.ItemWithVariant.cs`, `ManagementClient.Asset.cs`).
- **Request models are serialized directly**: Model properties with `[JsonProperty]` attributes map directly to the API contract. The model shape must match the API exactly.
- **Tests use JSON fixtures**: Tests create mock clients via `FileSystemFixture.CreateMockClientWithResponse("FileName.json")` loading from `Data/` subdirectories.
- **References**: The `Reference` class supports `ById`, `ByCodename`, and `ByExternalId` factory methods for identifying API resources.

## Commits

Use the format `TICKET-ID - Description` (e.g. `EN-713 - Add component_types filter`). Branch names follow `TICKET-ID_Short_description`.

## Versioning

Package version is set in `Kontent.Ai.Management.csproj` via `<Version>` property.
