using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManager.Application.Users.Commands.CreateUser;
using ResourceManager.Application.Users.Commands.DeleteUser;
using ResourceManager.Application.Users.Queries.GetUser;
using ResourceManager.Application.Users.Queries.ListUsers;
using ResourceManager.Contracts.Users;
using DomainUserRole = ResourceManager.Domain.Users.UserRole;
using User = ResourceManager.Domain.Users.User;

namespace ResourceManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }


    /// <summary>
    /// Lists all users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var query = new ListUsersQuery();
        var result = await _mediator.Send(query);
        var users = result
            .Select(user => new UserResponse(
                user.Id,
                user.Email,
                ToDto(user.Role)));
        return Ok(users);
    }

    /// <summary>
    /// Retrieves existing user
    /// </summary>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid userId)
    {
        var query = new GetUserQuery(userId);
        var result = await _mediator.Send(query);
        return result.Match(
            user => Ok(ToDto(user)),
            Problem);
    }

    /// <summary>
    /// Creates a new user with given or auto-generated Id
    /// </summary>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddUserRequest request)
    {
        if (!DomainUserRole.TryFromName(request.Role.ToString(), out var role))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Invalid user role");
        }

        var command = new CreateUserCommand(role, request.Email);

        var createUserResult = await _mediator.Send(command);

        return createUserResult.Match(
            user => CreatedAtAction(
                nameof(Get),
                new { userId = user.Id },
                ToDto(user)),
            Problem);
    }

    /// <summary>
    /// Deletes existing user from the database
    /// </summary>
    [Authorize]
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid userId)
    {
        var command = new DeleteUserCommand(userId);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    private static UserRole ToDto(DomainUserRole userRole)
    {
        return userRole.Name switch
        {
            nameof(DomainUserRole.User) => UserRole.User,
            nameof(DomainUserRole.Admin) => UserRole.Admin,
            _ => throw new InvalidOperationException()
        };
    }

    private static UserResponse ToDto(User user)
    {
        return new UserResponse(user.Id, user.Email, ToDto(user.Role));
    }
}