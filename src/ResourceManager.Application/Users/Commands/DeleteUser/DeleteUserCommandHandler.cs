using ErrorOr;
using MediatR;

namespace ResourceManager.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<Deleted>>
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _usersRepository.ExistsAsync(request.Id))
            return Error.NotFound();

        var user = await _usersRepository.GetByIdAsync(request.Id);
        await _usersRepository.RemoveAsync(user!);
        var result = await _usersRepository.CommitChangesAsync();

        return result.Match<ErrorOr<Deleted>>(
            _ => Result.Deleted,
            error => error);
    }
}