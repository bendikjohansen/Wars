#!/bin/zsh

dotnet ef database drop -s Src/Wars.Api -c ResourcesDbContext
dotnet ef migrations remove -s Src/Wars.Api -p Src/Wars.Resources -c ResourcesDbContext
dotnet ef migrations add InitialResources -s Src/Wars.Api -p Src/Wars.Resources -c ResourcesDbContext -o Infrastructure/Data/Migrations
dotnet ef database update -s Src/Wars.Api -c VillagesDbContext
dotnet ef database update -s Src/Wars.Api -c UsersDbContext
dotnet ef database update -s Src/Wars.Api -c ResourcesDbContext
