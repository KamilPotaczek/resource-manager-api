using ErrorOr;
using MediatR;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Queries.GetResource;

internal sealed class GetResourceQueryHandler : IRequestHandler<GetResourceQuery, ErrorOr<Resource>>
{
    private readonly IResourcesRepository _resourcesRepository;

    public GetResourceQueryHandler(IResourcesRepository resourcesRepository)
    {
        _resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
    }

    public async Task<ErrorOr<Resource>> Handle(GetResourceQuery request, CancellationToken cancellationToken)
    {
        var resource = await _resourcesRepository.GetByIdAsync(request.ResourceId);
        if (resource is null)
            return Error.NotFound("Resource with given Id not found");

        return resource;
    }
}