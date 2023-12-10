using CleanArchitecture.Application.SubcutaneousTests.Common;

using FluentAssertions;

using MediatR;

using TestCommon.Subscriptions;
using TestCommon.TestConstants;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionTests(MediatorFactory mediatorFactory)
    : IClassFixture<MediatorFactory>
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task CreateSubscription_WhenNoSubscription_ShouldCreateSubscription()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(Constants.User.Id);
        result.Value.SubscriptionType.Should().Be(Constants.Subscription.Type);
    }

    [Fact]
    public async Task CreateSubscription_WhenSubscriptionAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var firstResult = await _mediator.Send(command);
        var secondResult = await _mediator.Send(command);

        // Assert
        firstResult.IsError.Should().BeFalse();
        firstResult.Value.UserId.Should().Be(Constants.User.Id);
        firstResult.Value.SubscriptionType.Should().Be(Constants.Subscription.Type);

        secondResult.IsError.Should().BeTrue();
        secondResult.FirstError.Type.Should().Be(ErrorOr.ErrorType.Conflict);
    }
}