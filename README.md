<div align="center">

[![Build](https://github.com/amantinband/clean-architecture/actions/workflows/build.yml/badge.svg)](https://github.com/amantinband/clean-architecture/actions/workflows/build.yml)

[![GitHub contributors](https://img.shields.io/github/contributors/amantinband/clean-architecture)](https://GitHub.com/amantinband/clean-architecture/graphs/contributors/) [![GitHub Stars](https://img.shields.io/github/stars/amantinband/clean-architecture.svg)](https://github.com/amantinband/clean-architecture/stargazers) [![GitHub license](https://img.shields.io/github/license/amantinband/clean-architecture)](https://github.com/amantinband/clean-architecture/blob/main/LICENSE)
[![codecov](https://codecov.io/gh/amantinband/clean-architecture/branch/main/graph/badge.svg?token=DR2EBIWK7B)](https://codecov.io/gh/amantinband/clean-architecture)

---

![Clean Architecture Template Title](assets/Clean%20Architecture%20Template%20Title.png)


---

</div>

```shell
dotnet add package Amantinband.CleanArchitecture.Template --version 1.0.0

dotnet new clean-arch -o CleanArchitecture
```


- [️Important notice ⚠️](#️important-notice-️)
- [Give it a star ⭐](#give-it-a-star-)
- [Domain Overview 🌍](#domain-overview-)
  - [Basic Subscription](#basic-subscription)
  - [Pro Subscription](#pro-subscription)
- [Use Cases / Features 🤓](#use-cases--features-)
  - [Subscriptions](#subscriptions)
  - [Reminders](#reminders)
- [Getting Started 🏃](#getting-started-)
- [Folder Structure 📁](#folder-structure-)
- [Authorization 🔐](#authorization-)
  - [Authorization Types](#authorization-types)
    - [Role-Based Authorization](#role-based-authorization)
    - [Permission-Based Authorization](#permission-based-authorization)
    - [Policy-Based Authorization](#policy-based-authorization)
  - [Mixing Authorization Types](#mixing-authorization-types)
- [Testing 📝](#testing-)
  - [Test Types](#test-types)
    - [Domain Layer Unit Tests](#domain-layer-unit-tests)
    - [Application Layer Unit Tests](#application-layer-unit-tests)
    - [Application Layer Subcutaneous Tests](#application-layer-subcutaneous-tests)
    - [Presentation Layer Integration Tests](#presentation-layer-integration-tests)
- [Contribution 🤲](#contribution-)
- [Credits 🙏](#credits-)
- [License 🪪](#license-)

# ️Important notice ⚠️

This template is still under construction 👷.

If you're interested in learning more about clean architecture and how to build production applications structured following clean architecture using .NET, check out my comprehensive [course](https://dometrain.com/bundle/from-zero-to-hero-clean-architecture) on Dometrain where I cover everything you need to know when building production applications structured following clean architecture.

# Give it a star ⭐

Loving it? Show your support by giving this project a star!

# Domain Overview 🌍

This is a simple reminder application. It allows users to create and manage their reminders.

To create reminders, a user must have an active subscription.

## Basic Subscription

Users with a basic subscription can create up to 3 daily reminders.

## Pro Subscription

Users with a pro subscription do not have a daily limit on the number of reminders.

# Use Cases / Features 🤓

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

# Getting Started 🏃

1. Run the project `dotnet run --project src/CleanArchitecture.Api`
1. Navigate to `requests/Tokens/GenerateToken.http` and generate a token.

    ```yaml
    POST {{host}}/tokens/generate
    Content-Type: application/json
    ```

    ```http
    {
        "Id": "bae93bf5-9e3c-47b3-aace-3034653b6bb2",
        "FirstName": "Amichai",
        "LastName": "Mantinband",
        "Email": "amichai@mantinband.com",
        "Permissions": [
            "set:reminder",
            "get:reminder",
            "dismiss:reminder",
            "delete:reminder",
            "create:subscription",
            "delete:subscription",
            "get:subscription"
        ],
        "Roles": [
            "Admin"
        ]
    }
    ```

    > Note: Since most systems use an external identity provider, this project uses a simple token generator endpoint that generates a token based on the details you provide. This is a simple way to generate a token for testing purposes and is closer to how your system will likely be designed when using an external identity provider.

1. Create a subscription

    ```yaml
    POST {{host}}/users/{{userId}}/subscriptions
    Content-Type: application/json
    Authorization: Bearer {{token}}
    ```

    ```http
    {
        "SubscriptionType": "Basic"
    }
    ```

    > Note: To replace http file variables `{{variableName}}`, you can either:
    >   1. Use the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension for VS Code + update the values under ".vscode/settings.json". This will update the value for all http files.
    >   1. Define the variables in the http file itself:
    >       ```yaml
    >       @host = http://localhost:5001
    >       @userId = bae93bf5-9e3c-47b3-aace-3034653b6bb2
    >       ```
    >   1. Replace the variables manually.

1. Create a reminder

    ```yaml
    POST {{host}}/users/{{userId}}/subscriptions/{{subscriptionId}}/reminders
    Content-Type: application/json
    Authorization: Bearer {{token}}
    ```

    ```http
    {
        "text": "let's do it",
        "dateTime": "2025-2-26"
    }
    ```

# Folder Structure 📁

![Folder structure](assets/Clean%20Architecture%20Template.png)

# Authorization 🔐

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

To apply permission based authorization, use the `Authorize` attribute with the `Permissions` parameter and implement the `IAuthorizeableRequest` interface.

For example:

```csharp
[Authorize(Permissions = "get:reminder")]
public record GetReminderQuery(Guid UserId, Guid SubscriptionId, Guid ReminderId) : IAuthorizeableRequest<ErrorOr<Reminder>>;
```

Will only allow users with the `get:reminder` permission to get a subscription.

### Policy-Based Authorization

To apply policy based authorization, use the `Authorize` attribute with the `Policy` parameter and implement the `IAuthorizeableRequest` interface.

For example:

```csharp
[Authorize(Policies = "SelfOrAdmin")]
public record GetReminderQuery(Guid UserId, Guid SubscriptionId, Guid ReminderId) : IAuthorizeableRequest<ErrorOr<Reminder>>;
```

Will only allow users who pass the `SelfOrAdmin` policy to get a subscription.

Each policy is implemented as a simple method in the `PolicyEnforcer` class.

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

# Testing 📝

This project puts an emphasis on testability and comes with a comprehensive test suite.

![](assets/Clean%20Architecture%20Template%20Testing%20Suite.png)

## Test Types

### Domain Layer Unit Tests

The domain layer is tested using unit tests.
By the bare minimum, each domain entity should have a test that verifies its invariants.

![Domain Layer unit tests](assets/Clean%20Architecture%20Template%20Domain%20Layer%20Unit%20Tests.png)

### Application Layer Unit Tests

The domain layer is tested using both unit tests and subcutaneous tests.

Since each one of the application layer use cases has its corresponding subcutaneous test, the unit tests are used to test the application layer standalone components, such as the `ValidationBehavior` and the `AuthorizationBehavior`.

![Application Layer unit tests](assets/Clean%20Architecture%20Template%20Application%20Layer%20Unit%20Tests.png)

### Application Layer Subcutaneous Tests

Subcutaneous tests are tests that operate right under the presentation layer.
These tests are responsible for testing the core logic of our application, which is the application layer and the domain layer.

The reason there are so many of these tests, is because each one of the application layer use cases has its corresponding subcutaneous test.

This allows us to test the application layer and the domain layer based on the actual expected usage, giving us the confidence that our application works as expected and that the system cannot be manipulated in a way we don't allow.

I recommend spending more effort on these tests than the other tests, since they aren't too expensive to write, and the value they provide is huge.

![](assets/Clean%20Architecture%20Template%20Subcutaneous%20Tests.png)

### Presentation Layer Integration Tests

The api layer is tested using integration tests. This is where we want to cover the entire system, including the database, external dependencies and the presentation layer.

Unlike the subcutaneous tests, the focus of these tests is to ensure the integration between the various components of our system and other systems.

![Integration Tests](assets/Clean%20Architecture%20Template%20Integration%20Tests.png)

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# Credits 🙏

- [CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) - An awesome clean architecture solution template by Jason Taylor

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/mantinband/clean-architecture/blob/main/LICENSE) license.