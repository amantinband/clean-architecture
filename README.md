<div align="center">

[![NuGet](https://img.shields.io/nuget/v/erroror.svg)](https://www.nuget.org/packages/erroror)

[![Build](https://github.com/amantinband/error-or/actions/workflows/build.yml/badge.svg)](https://github.com/amantinband/error-or/actions/workflows/build.yml) [![publish ErrorOr to nuget](https://github.com/amantinband/error-or/actions/workflows/publish.yml/badge.svg)](https://github.com/amantinband/error-or/actions/workflows/publish.yml)

[![GitHub contributors](https://img.shields.io/github/contributors/amantinband/error-or)](https://GitHub.com/amantinband/error-or/graphs/contributors/) [![GitHub Stars](https://img.shields.io/github/stars/amantinband/error-or.svg)](https://github.com/amantinband/error-or/stargazers) [![GitHub license](https://img.shields.io/github/license/amantinband/error-or)](https://github.com/amantinband/error-or/blob/main/LICENSE)
[![codecov](https://codecov.io/gh/amantinband/error-or/branch/main/graph/badge.svg?token=DR2EBIWK7B)](https://codecov.io/gh/amantinband/error-or)

</div>

# Important notice

If you like this template, you may also enjoy my [comprehensive course](https://dometrain.com/bundle/from-zero-to-hero-clean-architectur) on Dometrain where I cover everything you need to know about building production applications structured following clean architecture.

# Give it a star ⭐

Loving it? Show your support by giving this project a star!

# Domain Overview

This is a simple reminder application. It allows users to create and manage their reminders.

To create reminders, a user must have an active subscription.

## Basic Subscription

Users with a basic subscription can create up to 3 daily reminders.

## Pro Subscription

Users with a pro subscription do not have a daily limit on the number of reminders.

# Use Cases / Features

## Subscriptions

1. Create Subscription
1. Cancel Subscription
1. Get Subscription

## Reminders

1. Delete Reminder
1. Dismiss Reminder
1. Set Reminder
1. Get Reminder
1. List Reminders

# Folder Structure

![Folder structure](assets/Clean%20Architecture%20Template.svg)

# Authorization

This project puts an emphasis on complex authorization scenarios and supports *role-based*, *permission-based* and *policy-based* authorization.

## Authorization Types

### Role-Based Authorization

To apply role based authorization, use the `Authorize` attribute with the `Roles` parameter and implement the `IAuthorizeableRequest` interface.

For example:

```csharp
[Authorize(Roles = "Admin")]
public record CancelSubscriptionCommand(Guid UserId, Guid SubscriptionId) : IAuthorizeableRequest<ErrorOr<Success>>;
```

Will only allow users with the `Admin` role to cancel subscriptions.

### Permission-Based Authorization

To apply role based authorization, use the `Authorize` attribute with the `Permissions` parameter and implement the `IAuthorizeableRequest` interface.

For example:

```csharp
[Authorize(Permissions = "get:reminder")]
public record GetReminderQuery(Guid UserId, Guid SubscriptionId, Guid ReminderId) : IAuthorizeableRequest<ErrorOr<Reminder>>;
```

Will only allow users with the `get:reminder` permission to get a subscription.

### Policy-Based Authorization

To apply role based authorization, use the `Authorize` attribute with the `Policy` parameter and implement the `IAuthorizeableRequest` interface.

For example:

```csharp
[Authorize(Policies = "SelfOrAdmin")]
public record GetReminderQuery(Guid UserId, Guid SubscriptionId, Guid ReminderId) : IAuthorizeableRequest<ErrorOr<Reminder>>;
```

Will only allow users who pass the `SelfOrAdmin` policy to get a subscription.

Each policy is implemented as a method in the `IPolicyEnforcer` interface.

The policy "SelfOrAdmin" for example, can be implemented as follows:

```csharp
public class PolicyEnforcer : IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy)
    {
        return policy switch
        {
            "SelfOrAdmin" => SelfOrAdminPolicy(request, currentUser),
            _ => Error.Unexpected(description: "Unknown policy name"),
        };
    }

    private static ErrorOr<Success> SelfOrAdminPolicy<T>(IAuthorizeableRequest<T> request, CurrentUser currentUser) =>
        request.UserId == currentUser.Id || currentUser.Roles.Contains(Role.Admin)
            ? Result.Success
            : Error.Unauthorized(description: "Requesting user failed policy requirement");
}
```

## Mixing Authorization Types

You can mix and match authorization types to create complex authorization scenarios.

For example:

```csharp
[Authorize(Permissions = "get:reminder,list:reminder", Policies = "SelfOrAdmin", Roles = "ReminderManager")]
public record ListRemindersQuery(Guid UserId, Guid SubscriptionId, Guid ReminderId) : IAuthorizeableRequest<ErrorOr<Reminder>>;
```

Will only allow users with the `get:reminder` permission or the `list:reminder` permission, and who pass the `SelfOrAdmin` policy, and who have the `ReminderManager` role to list reminders.

Another option, is specifying the `Authorize` attribute multiple times:

```csharp
[Authorize(Permissions = "get:reminder")]
[Authorize(Permissions = "list:reminder")]
[Authorize(Policies = "SelfOrAdmin")]
[Authorize(Roles = "ReminderManager")]
public record ListRemindersQuery(Guid UserId, Guid SubscriptionId, Guid ReminderId) : IAuthorizeableRequest<ErrorOr<Reminder>>;
```

# Testing

This project puts an emphasis on testability and comes with a comprehensive test suite.

![Testing Suite](assets/Clean%20Architecture%20Template%20Testing%20Suite.svg)

## Test Types

### Domain Layer Unit Tests

The domain layer is tested using unit tests.
By the bare minimum, each domain entity should have a test that verifies its invariants.

### Application Layer Unit Tests

The domain layer is tested using both unit tests and subcutaneous tests.

Since each one of the application layer use cases has its corresponding subcutaneous test, the unit tests are used to test the application layer standalone components, such as the `ValidationBehavior` and the `AuthorizationBehavior`.

### Application Layer Subcutaneous Tests

Subcutaneous tests are tests that operate right under the presentation layer.
These tests are responsible for testing the core logic of our application, which is the application layer and the domain layer.

The reason there are so many of these tests, is because each one of the application layer use cases has its corresponding subcutaneous test.

This allows us to test the application layer and the domain layer based on the actual expected usage, giving us the confidence that our application works as expected and that the system cannot be manipulated in a way we don't allow.

I recommend spending more effort on these tests than the other tests, since they aren't too expensive to write, and the value they provide is huge.

### Presentation Layer Integration Tests

The api layer is tested using integration tests. This is where we want to cover the entire system, including the database, external dependencies and the presentation layer.

Unlike the subcutaneous tests, these tests are not meant to test the core logic of our application, but rather to test the integration between the various components of our system and other systems.

# Contribution

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# Credits

- [CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) - An awesome clean architecture solution template by Jason Taylor

# License

This project is licensed under the terms of the [MIT](https://github.com/mantinband/error-or/blob/main/LICENSE) license.