using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Wars.Buildings.Domain;
using Wars.Buildings.Infrastructure.Data;

namespace Wars.Buildings.Features;

internal static class ProcessBuildingLevels
{
    internal class Endpoint(BuildingsContext context, TimeProvider timeProvider) : Endpoint<Request, Response>
    {
        private readonly BuildingsContext _context = context;

        public override void Configure()
        {
            Claims("UserId");
            Post("process-buildings");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var village = await _context.Villages
                .Include(v => v.UpgradeQueue)
                .SingleOrDefaultAsync(v => v.Id == req.VillageId, ct);

            if (village is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var now = timeProvider.GetUtcNow();
            village.ProcessFinishedUpgrades(now);
            await _context.SaveChangesAsync(ct);

            var levels = village.BuildingLevels;
            var response = new Response
            {
                VillageId = village.Id,
                BuildingLevels = new Response.BuildingRegistry
                {
                    Headquarter = levels.Headquarter,
                    Warehouse = levels.Warehouse,
                    ClayPit = levels.ClayPit,
                    IronMine = levels.IronMine,
                    LumberCamp = levels.LumberCamp
                },
                UpgradeQueue = village.UpgradeQueue.Select(item => new Response.BuildingUpgrade
                {
                    Building = item.Building switch
                    {
                        BuildingType.Headquarter => Response.BuildingUpgrade.BuildingType.Headquarter,
                        BuildingType.Warehouse => Response.BuildingUpgrade.BuildingType.Warehouse,
                        BuildingType.ClayPit => Response.BuildingUpgrade.BuildingType.ClayPit,
                        BuildingType.IronMine => Response.BuildingUpgrade.BuildingType.IronMine,
                        BuildingType.LumberCamp => Response.BuildingUpgrade.BuildingType.LumberCamp,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Cost = new Response.BuildingUpgrade.ResourceCollection
                    {
                        Clay = item.Cost.Clay,
                        Iron = item.Cost.Iron,
                        Wood = item.Cost.Wood
                    },
                    TimeLeft = item.TimeLeft(timeProvider.GetUtcNow()),
                    Started = item.Started(timeProvider.GetUtcNow())
                }).ToList()
            };
            await SendAsync(response, cancellation: ct);
        }
    }

    internal record Request(string VillageId);

    internal record Response
    {
        public required string VillageId { get; init; }
        public required BuildingRegistry BuildingLevels { get; init; }
        public required List<BuildingUpgrade> UpgradeQueue { get; init; }

        internal record BuildingRegistry
        {
            public required int ClayPit { get; init; }
            public required int LumberCamp { get; init; }
            public required int IronMine { get; init; }
            public required int Warehouse { get; init; }
            public required int Headquarter { get; init; }
        }

        internal record BuildingUpgrade
        {
            public Guid Id { get; private init; } = Guid.NewGuid();
            public required BuildingType Building { get; init; }
            public required ResourceCollection Cost { get; init; }
            public required TimeSpan TimeLeft { get; init; }
            public required bool Started { get; init; }

            internal enum BuildingType
            {
                ClayPit,
                IronMine,
                LumberCamp,
                Warehouse,
                Headquarter
            }

            internal record ResourceCollection
            {
                public required int Clay { get; init; }
                public required int Iron { get; init; }
                public required int Wood { get; init; }
            }
        }
    }
}
