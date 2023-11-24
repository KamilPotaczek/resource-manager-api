using ErrorOr;
using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<User>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ErrorOr<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _usersRepository.ExistsAsync(request.Email))
            return Error.Conflict(description: "User already exists");

        var user = new User(request.Email, request.Role);
        await _usersRepository.AddAsync(user);
        await _unitOfWork.CommitChangesAsync();
        return user;
    }
}