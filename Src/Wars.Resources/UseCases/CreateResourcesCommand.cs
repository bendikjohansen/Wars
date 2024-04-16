using Ardalis.Result;
using MediatR;

namespace Wars.Resources.UseCases;

internal record CreateResourcesCommand(string VillageId) : IRequest<Result>;

internal class CreateResourceCommandHandler(IResourcesRepository repository) : IRequestHandler<CreateResourcesCommand, Result>
{
    private readonly IResourcesRepository _repository = repository;

    public async Task<Result> Handle(CreateResourcesCommand request, CancellationToken ct)
    {
        var existingVillage = await _repository.GetAsync(request.VillageId, ct);
        if (existingVillage is not null)
        {
            return Result.Error($"Resources already exists for village with ID {request.VillageId}.");
        }

        var resources = new Domain.Resources
        {
            VillageId = request.VillageId
        };
        _repository.Add(resources);
        await _repository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
