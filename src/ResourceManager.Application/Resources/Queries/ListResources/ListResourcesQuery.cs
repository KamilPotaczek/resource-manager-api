using ErrorOr;
using MediatR;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Queries.ListResources;

public record ListResourcesQuery : IRequest<List<Resource>>;