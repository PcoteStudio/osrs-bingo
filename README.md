# Bingo-Style Event Manager for OldSchool RuneScape

## Website

- Users
  - CRUD
  - Authentication
  - Roles
- Team
  - CRUD
  - Display team statistics
- Player
  - CRUD
  - Display player statistics
- Board
  - Display the event board and its tiles
  - Display board statistics
- Tile
  - CRUD
  - Display the dynamic amount of points the tile is worth
  - Display an image of the tile content
  - Display the tile name and description
  - Display the team or player who completed the tile
  - Mark a tile as completed and indicate which player/team did it

## API

## Planned features

- Multiple event formats
  - Classic bingo
  - Multi-layered bingo
  - Snakes and ladders
- Multiple tile types
  - PvM drops
  - XP gains
  - Multi-constraints
- Teams and players management
- Events and tiles planning
  - Fetch images automatically
  - Calculate EH for tile completion
- Statistics
  - Tiles count
  - Drops value
  - EHP
  - EHB

## TODO

- Implement [Verify](https://github.com/VerifyTests/Verify) in feature tests to validate that the output of the API doesn't change unexpectedly between versions.

## TO RUN

```bash
winget upgrade Microsoft.DotNet.SDK.9
dotnet restore
```

## Entity Framework

### Add a new migration

```bash
cd bingo-api
dotnet.exe ef migrations add NameOfTheMigration --project src\Bingo.Api.Data\Bingo.Api.Data.csproj --startup-project src\Bingo.Api.Web\Bingo.Api.Web.csproj --context Bingo.Api.Data.ApplicationDbContext --configuration Debug --verbose --output-dir Migrations
```

### Updating the databases

Both the local and test databases are automatically migrated upon launching their respective projects.
