using ErrorOr;
using MediatR;
using ResourceManager.Application.Common;

namespace ResourceManager.Application.Resources.Commands.LockResource;

internal sealed class LockResourceCommandHandler : IRequestHandler<LockResourceCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IResourcesRepository _resourcesRepository;

    public LockResourceCommandHandler(
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider,
        IResourcesRepository resourcesRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        _resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
    }

    public async Task<ErrorOr<Success>> Handle(LockResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _resourcesRepository.GetByIdAsync(request.ResourceId);
        if (resource is null)
            return Error.NotFound(description: $"Resource with Id: {request.ResourceId} not found");

        var result = resource.Lock(request.user, request.Until, _dateTimeProvider.UtcNow);
        if (resource.ResourceLock is null)
            return Error.Unexpected(description: "Failed to create Resource Lock - try again later");

        if (result.IsError) return result;
        
        await _resourcesRepository.UpdateAsync(resource);
        await _unitOfWork.CommitChangesAsync();
        return Result.Success;
    }
}