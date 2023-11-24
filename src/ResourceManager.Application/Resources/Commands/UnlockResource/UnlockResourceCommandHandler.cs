using ErrorOr;
using MediatR;
using ResourceManager.Application.Common;

namespace ResourceManager.Application.Resources.Commands.UnlockResource;

internal sealed class UnlockResourceCommandHandler : IRequestHandler<UnlockResourceCommand, ErrorOr<Success>>
{
    private readonly IResourcesRepository _resourcesRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UnlockResourceCommandHandler(
        IResourcesRepository resourcesRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<ErrorOr<Success>> Handle(UnlockResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _resourcesRepository.GetByIdAsync(request.ResourceId);
        if (resource is null)
            return Error.NotFound(description: $"Resource with Id: {request.ResourceId} not found");

        var result = resource.Unlock(request.User, _dateTimeProvider.UtcNow);
        if (result.IsError)
            return result;

        await _resourcesRepository.UpdateAsync(resource);
        return await _resourcesRepository.CommitChangesAsync();
    }
}