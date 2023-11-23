using ErrorOr;
using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Users.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<ErrorOr<User>>;