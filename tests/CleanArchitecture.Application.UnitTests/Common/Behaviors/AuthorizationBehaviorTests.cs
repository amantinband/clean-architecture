using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;

using MediatR;

namespace CleanArchitecture.Application.UnitTests.Common.Behaviors;

public class AuthorizationBehaviorTests
{
    private readonly IAuthorizationService _mockAuthorizationService;
    private readonly RequestHandlerDelegate<ErrorOr<Response>> _mockNextBehavior;

    public AuthorizationBehaviorTests()
    {
        _mockAuthorizationService = Substitute.For<IAuthorizationService>();

        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Response>>>();
        _mockNextBehavior
            .Invoke()
            .Returns(Response.Instance);
    }

    [Fact]
    public async Task InvokeAuthorizationBehavior_WhenNoAuthorizationAttribute_ShouldInvokeNextBehavior()
    {
        // Arrange
        var request = new RequestWithNoAuthorizationAttribute(Constants.User.Id);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithNoAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Response.Instance);
    }

    [Fact]
    public async Task InvokeAuthorizationBehavior_WhenSingleAuthorizationAttributeAndUserIsAuthorized_ShouldInvokeNextBehavior()
    {
        // Arrange
        var request = new RequestWithSingleAuthorizationAttribute(Constants.User.Id);

        _mockAuthorizationService
            .AuthorizeCurrentUser(
                request,
                Must.BeEmptyList<string>(),
                Must.BeListWith(["Permission"]),
                Must.BeEmptyList<string>())
            .Returns(Result.Success);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithSingleAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Response.Instance);
    }

    [Fact]
    public async Task InvokeAuthorizationBehavior_WhenSingleAuthorizationAttributeAndUserIsNotAuthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new RequestWithSingleAuthorizationAttribute(Constants.User.Id);

        var error = Error.Unauthorized(code: "bad.user", description: "bad user");

        _mockAuthorizationService
            .AuthorizeCurrentUser(
                request,
                Must.BeEmptyList<string>(),
                Must.BeListWith(["Permission"]),
                Must.BeEmptyList<string>())
            .Returns(error);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithSingleAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public async Task InvokeAuthorizationBehavior_WhenTonsOfAuthorizationAttributesAndUserIsAuthorized_ShouldInvokeNextBehavior()
    {
        // Arrange
        var request = new RequestWithTonsOfAuthorizationAttribute(Constants.User.Id);

        _mockAuthorizationService
            .AuthorizeCurrentUser(
                request,
                Must.BeListWith(["Role1", "Role2", "Role3"]),
                Must.BeListWith(["Permission1", "Permission2", "Permission3"]),
                Must.BeListWith(["Policy1", "Policy2", "Policy3"]))
            .Returns(Result.Success);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithTonsOfAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Response.Instance);
    }

    [Fact]
    public async Task InvokeAuthorizationBehavior_WhenTonsOfAuthorizationAttributesAndUserIsNotAuthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new RequestWithTonsOfAuthorizationAttribute(Constants.User.Id);

        var error = Error.Unauthorized(code: "bad.user", description: "bad user");

        _mockAuthorizationService
            .AuthorizeCurrentUser(
                request,
                Must.BeListWith(["Role1", "Role2", "Role3"]),
                Must.BeListWith(["Permission1", "Permission2", "Permission3"]),
                Must.BeListWith(["Policy1", "Policy2", "Policy3"]))
            .Returns(error);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithTonsOfAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }
}

public record Response
{
    public static readonly Response Instance = new();
}

public record RequestWithNoAuthorizationAttribute(Guid UserId) : IAuthorizeableRequest<ErrorOr<Response>>;

[Authorize(Permissions = "Permission")]
public record RequestWithSingleAuthorizationAttribute(Guid UserId) : IAuthorizeableRequest<ErrorOr<Response>>;

[Authorize(Permissions = "Permission1,Permission2")]
[Authorize(Roles = "Role1,Role2")]
[Authorize(Policies = "Policy1,Policy2")]
[Authorize(Permissions = "Permission3", Roles = "Role3", Policies = "Policy3")]
public record RequestWithTonsOfAuthorizationAttribute(Guid UserId) : IAuthorizeableRequest<ErrorOr<Response>>;