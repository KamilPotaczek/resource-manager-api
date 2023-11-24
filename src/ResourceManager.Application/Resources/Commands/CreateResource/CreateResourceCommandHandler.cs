using ErrorOr;
using MediatR;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Commands.CreateResource;

internal sealed class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, ErrorOr<Resource>>
{
    private readonly IResourcesRepository _resourcesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateResourceCommandHandler(IResourcesRepository resourcesRepository, IUnitOfWork unitOfWork)
    {
        _resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ErrorOr<Resource>> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        if (request.ResourceId.HasValue && await _resourcesRepository.ExistsAsync(request.ResourceId.Value))
            return Error.Conflict(description: $"Resource with id: {request.ResourceId} already exists");

        var resource = new Resource(request.ResourceId);
        await _resourcesRepository.AddAsync(resource);
        await _unitOfWork.CommitChangesAsync();
        return resource;
    }
}