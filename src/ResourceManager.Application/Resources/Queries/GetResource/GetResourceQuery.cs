using ErrorOr;
using MediatR;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Queries.GetResource;

public record GetResourceQuery(Guid ResourceId) : IRequest<ErrorOr<Resource>>;