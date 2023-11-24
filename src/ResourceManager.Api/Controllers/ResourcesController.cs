using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManager.Application.Common;
using ResourceManager.Application.Resources.Commands.CreateResource;
using ResourceManager.Application.Resources.Commands.LockResource;
using ResourceManager.Application.Resources.Commands.UnlockResource;
using ResourceManager.Application.Resources.Commands.WithdrawResource;
using ResourceManager.Application.Resources.Queries.GetResource;
using ResourceManager.Application.Resources.Queries.ListResources;
using ResourceManager.Contracts.Resources;
using ResourceManager.Domain.Resources;
using ResourceManager.Domain.Users;

namespace ResourceManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class ResourcesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContextProvider _userContextProvider;

    public ResourcesController(ISender mediator, IDateTimeProvider dateTimeProvider, IUserContextProvider userContextProvider)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        _userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
    }
    
    /// <summary>
    /// Creates a new resource with given or auto-generated Id
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Add([FromBody] AddResourceRequest request)
    {
        var user = _userContextProvider.GetUserFromContext();
        if (user.Role != UserRole.Admin)
            return Problem(Error.Unauthorized(description: "Only Admin user can create a resource"));

        var command = new CreateResourceCommand(request.Id, user.Role);

        var createdResult = await _mediator.Send(command);

        return createdResult.Match(
            resource => Ok(ToDto(resource)),
            Problem);
    }

    /// <summary>
    /// Returns existing resource
    /// </summary>
    [HttpGet("{resourceId:guid}")]
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid resourceId)
    {
        var query = new GetResourceQuery(resourceId);

        var result = await _mediator.Send(query);

        return result.Match(
            resource => Ok(ToDto(resource)),
            Problem);
    }

    /// <summary>
    /// Locks a resource for calling user or updates lock period if lock already exists
    /// </summary>
    [HttpPut("{resourceId:guid}/lock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LockResource(Guid resourceId, [FromBody] LockResourceRequest request)
    {
        var user = _userContextProvider.GetUserFromContext();
        var command = new LockResourceCommand(user, resourceId, request.Until);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    /// <summary>
    /// Unlocks a resource. Only lock owner can unlock a resource
    /// </summary>
    [HttpPut("{resourceId:guid}/unlock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LockResource(Guid resourceId)
    {
        var user = _userContextProvider.GetUserFromContext();
        var command = new UnlockResourceCommand(user, resourceId);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    /// <summary>
    /// Withdraws a resource and removes its lock
    /// </summary>
    [HttpPut("{resourceId:guid}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> WithdrawResource(Guid resourceId)
    {
        var user = _userContextProvider.GetUserFromContext();
        var command = new WithdrawResourceCommand(user, resourceId);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    /// <summary>
    /// Lists all resources
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var query = new ListResourcesQuery();

        var result = await _mediator.Send(query);

        return Ok(result.Select(ToDto));
    }

    private ResourceResponse ToDto(Resource resource)
    {
        return new ResourceResponse(resource.Id, resource.IsWithdrawn, resource.IsLocked(_dateTimeProvider.UtcNow));
    }
}