using Ardalis.Result;
using MediatR;
using Wars.Resources.Domain;

namespace Wars.Resources.UseCases;

internal record CollectResourcesCommand(string VillageId) : IRequest<Result<Village>>;

internal class CollectResourcesCommandHandler(IResourcesRepository resourcesRepository) : IRequestHandler<CollectResourcesCommand, Result<Village>>
{
    private readonly IResourcesRepository _resourcesRepository = resourcesRepository;

    public async Task<Result<Village>> Handle(CollectResourcesCommand request, CancellationToken ct)
    {
        var village = await _resourcesRepository.GetAsync(request.VillageId, ct);
        if (village is null)
        {
            return Result.NotFound();
        }

        village.CollectResources(DateTimeOffset.UtcNow);
        await _resourcesRepository.SaveChangesAsync(ct);
        return village;
    }
}
