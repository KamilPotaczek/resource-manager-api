using ErrorOr;
using MediatR;
using ResourceManager.Domain.Resources;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Resources.Commands.CreateResource;

public record CreateResourceCommand(Guid? ResourceId, UserRole UserRole) : IRequest<ErrorOr<Resource>>;