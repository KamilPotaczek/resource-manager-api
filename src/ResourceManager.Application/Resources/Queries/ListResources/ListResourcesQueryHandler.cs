using ErrorOr;
using MediatR;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Queries.ListResources;

internal sealed class ListResourcesQueryHandler : IRequestHandler<ListResourcesQuery, List<Resource>>
{
    private readonly IResourcesRepository _resourcesRepository;

    public ListResourcesQueryHandler(IResourcesRepository resourcesRepository)
    {
        _resourcesRepository = resourcesRepository;
    }

    public async Task<List<Resource>> Handle(ListResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _resourcesRepository.ListAsync();
        return resources;
    }
}