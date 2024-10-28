using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace Wars.Villages.Features;

internal static class ListVillages
{
    internal class Endpoint(
        IVillagesRepository villagesRepository,
        ILogger<Endpoint> logger) : EndpointWithoutRequest<Response>
    {
        private readonly IVillagesRepository _villagesRepository = villagesRepository;
        private readonly ILogger<Endpoint> _logger = logger;

        public override void Configure()
        {
            Get("list-villages");
            Claims("UserId");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = User.FindFirst("UserId")!.Value;

            var villages = await _villagesRepository.ListByUserAsync(userId, ct);
            _logger.LogInformation("Found {VillageCount} villages for user.", villages.Count);

            var villageResponses = villages.Select(village => new VillageResponse(village.Id, village.Name));
            await SendAsync(new Response(villageResponses), cancellation: ct);
        }
    }

    internal record Response(IEnumerable<VillageResponse> Villages);

    internal record VillageResponse(Guid Id, string Name);
}
