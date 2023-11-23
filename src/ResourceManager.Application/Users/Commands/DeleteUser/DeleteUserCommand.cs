using ErrorOr;
using MediatR;

namespace ResourceManager.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;