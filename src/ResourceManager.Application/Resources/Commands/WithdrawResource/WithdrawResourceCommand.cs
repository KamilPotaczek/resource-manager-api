using ErrorOr;
using MediatR;
using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Resources.Commands.WithdrawResource;

public record WithdrawResourceCommand(User User, Guid ResourceId) : IRequest<ErrorOr<Success>>;