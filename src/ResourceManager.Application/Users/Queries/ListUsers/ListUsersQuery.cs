using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Users.Queries.ListUsers;

public record ListUsersQuery : IRequest<List<User>>;