using ErrorOr;
using MediatR;

namespace ResourceManager.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<Deleted>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _usersRepository.ExistsAsync(request.Id))
            return Error.NotFound();

        var user = await _usersRepository.GetByIdAsync(request.Id);
        await _usersRepository.RemoveAsync(user!);
        await _unitOfWork.CommitChangesAsync();
        return Result.Deleted;
    }
}