using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Resources.Contracts;

namespace Wars.Resources.Features;

internal static class Pay
{
    internal class Handler(
        IResourcesRepository repository,
        ILogger<Handler> logger,
        TimeProvider time) : IRequestHandler<PayCommand, Result>
    {
        public async Task<Result> Handle(PayCommand request, CancellationToken ct)
        {
            var village = await repository.GetAsync(request.VillageId, ct);
            if (village is null)
            {
                return Result.NotFound();
            }

            village.CollectResources(time.GetUtcNow());
            var canAfford = village.HasFunds(request.Clay, request.Iron, request.Wood);
            if (!canAfford)
            {
                logger.LogInformation("Village could not afford to pay resources.");
                return Result.Error();
            }

            village.Pay(request.Clay, request.Iron, request.Wood, time.GetUtcNow());
            await repository.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
