using ErrorOr;
using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Resources.Commands.UnlockResource;

public record UnlockResourceCommand(User User, Guid ResourceId) : IRequest<ErrorOr<Success>>;