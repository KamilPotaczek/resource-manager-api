using ErrorOr;
using MediatR;

namespace ResourceManager.Application.Resources.Commands.WithdrawResource;

internal sealed class WithdrawResourceCommandHandler : IRequestHandler<WithdrawResourceCommand, ErrorOr<Success>>
{
    private readonly IResourcesRepository _resourcesRepository;

    public WithdrawResourceCommandHandler(IResourcesRepository resourcesRepository)
    {
        _resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
    }

    public async Task<ErrorOr<Success>> Handle(WithdrawResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _resourcesRepository.GetByIdAsync(request.ResourceId);
        if (resource is null)
            return Error.NotFound(description: $"Resource with Id: {request.ResourceId} not found");

        var result = resource.Withdraw(request.User);

        if (result.IsError)
            return result;

        await _resourcesRepository.UpdateAsync(resource);
        return await _resourcesRepository.CommitChangesAsync();
    }
}