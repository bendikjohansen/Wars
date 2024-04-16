using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Villages.Domain;

namespace Wars.Villages.UseCases;

internal record CreateVillageCommand(string UserId, string Name) : IRequest<Result>;

internal class CreateVillageCommandHandler(
    IVillagesRepository villagesRepository,
    ILogger<CreateVillageCommandHandler> logger) : IRequestHandler<CreateVillageCommand, Result>
{
    private readonly IVillagesRepository _villagesRepository = villagesRepository;
    private readonly ILogger<CreateVillageCommandHandler> _logger = logger;

    public async Task<Result> Handle(CreateVillageCommand request, CancellationToken ct)
    {
        var villages = await _villagesRepository.ListByUserAsync(request.UserId, ct);

        if (villages.Any())
        {
            _logger.LogWarning("User {userId} has already created a village", request.UserId);
            return Result.Invalid();
        }

        var newVillage = Village.Factory.Create(request.UserId, request.Name);
        _villagesRepository.Add(newVillage);
        await _villagesRepository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
