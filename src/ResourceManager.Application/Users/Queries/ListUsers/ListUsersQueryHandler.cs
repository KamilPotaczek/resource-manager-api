using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Users.Queries.ListUsers;

internal sealed class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<User>>
{
    private readonly IUsersRepository _usersRepository;

    public ListUsersQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
    }

    public async Task<List<User>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        return await _usersRepository.ListAsync();
    }
}