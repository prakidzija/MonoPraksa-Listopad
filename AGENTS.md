# AGENTS.md

## Change approval
- Before making changes, inspect relevant files and present a concise plan. Wait for explicit user approval before editing files or running commands that modify anything.

## Boundaries
- Two independent .NET 10 APIs coexist: VolleyballApp and LiberOS. `VolleyballAppFrontend/` (React/Vite) targets VolleyballApp, not LiberOS. There is no root solution or shared task runner; use explicit project/solution paths.
- `praksa2026-listopad-g01/` is a separate nested Git repository, not tracked by the outer repository. Check its status/diff with `git -C praksa2026-listopad-g01 ...`; outer Git commands do not review its file changes.
- VolleyballApp's web project references the nested Model/Repository/Service/Controller projects and explicitly excludes their sources from its own compilation. Keep that separation. Controllers are loaded through `AddApplicationPart` in `VolleyballApp/Program.cs`.
- `PraksaDbContext` lives in `VolleyballApp.Repository/` but uses namespace `VolleyballApp.Model`.
- LiberOS is wired in `praksa2026-listopad-g01/LiberOS.Api/Program.cs`; catalogue registrations are delegated to `AddCatalogueServices()`. Common holds models/contracts, DAL holds repositories/context/migrations, Service holds business logic.

## Commands
Run these from the repository root unless otherwise stated:

| Task | Command |
| --- | --- |
| Build VolleyballApp and its layers | `dotnet build VolleyballApp/VolleyballApp.slnx` |
| Run VolleyballApp for the frontend | `dotnet run --project VolleyballApp/VolleyballApp.csproj --launch-profile https` |
| Build LiberOS and its layers | `dotnet build praksa2026-listopad-g01/LiberOS.Api/LiberOS.slnx` |
| Run LiberOS | `dotnet run --project praksa2026-listopad-g01/LiberOS.Api/LiberOS.Api.csproj --launch-profile https` |
| Run standalone console exercise | `dotnet run --project ConsoleApp1/ConsoleApp1.csproj` |

- In `VolleyballAppFrontend/`: install with `npm ci`; use `npm run dev`, `npm run lint`, and `npm run build`. Focused lint: `npx eslint src/pages/Players.jsx` (substitute the changed file).
- No test projects, test scripts, or typecheck script are present. Backend builds and frontend lint/build are the available verification steps; a successful `dotnet test` alone is not evidence of test coverage.

## Runtime setup and pitfalls
- Both APIs require external PostgreSQL configuration plus `Jwt:Key`, `Jwt:Issuer`, and `Jwt:Audience`; checked-in appsettings only contain logging/host settings. Both API projects declare `UserSecretsId`, so use project-scoped user secrets or environment variables, not committed credentials. Environment connection keys: `ConnectionStrings__PraksaDB` for VolleyballApp, `ConnectionStrings__DefaultConnectionString` for LiberOS; JWT keys use `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience`.
- JWT keys are **Base64-decoded in VolleyballApp** but **UTF-8 encoded in LiberOS**. Do not reuse auth setup blindly.
- VolleyballApp's `https` profile serves `https://localhost:7169` and `http://localhost:5170`; plain `dotnet run` selects the first (`http`) profile, which does not provide the frontend's HTTPS target. LiberOS uses HTTPS `7076` / HTTP `5177`.
- Vite proxies only `/api`, `/auth`, and `/Player` to `https://localhost:7169` (`secure: false`). Frontend requests use relative URLs through `src/services/api.js`, which reads the bearer token from `sessionStorage`. New API route prefixes need matching proxy configuration; the proxy is dev-server configuration, not a production API base URL.
- VolleyballApp smoke check: `GET https://localhost:7169/api/health`. Both APIs expose `/openapi/v1.json` only in Development.
- VolleyballApp SQL is in `DB/PraksaDB.sql`; it includes schema changes and seed inserts, not an idempotent migration runner. Preserve the database's `adress` spelling in mappings.
- LiberOS EF migrations live in `LiberOS.DAL/Migrations/`, not the API project. Its default schema and migration history schema are `g01`; EF operations need DAL as the target project and API as the startup project. Neither API calls `Migrate()` or `EnsureCreated()` on startup.
