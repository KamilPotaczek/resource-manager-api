using MediatR;
using ErrorOr;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    UserRole Role,
    string Email) : IRequest<ErrorOr<User>>;