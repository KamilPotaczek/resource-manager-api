using ErrorOr;
using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Resources.Commands.LockResource;

public record LockResourceCommand(User user, Guid ResourceId, DateTime? Until) : IRequest<ErrorOr<Success>>;