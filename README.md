<div align="center">

[![Build](https://github.com/amantinband/clean-architecture/actions/workflows/build.yml/badge.svg)](https://github.com/amantinband/clean-architecture/actions/workflows/build.yml) [![Publish template to NuGet](https://github.com/amantinband/clean-architecture/actions/workflows/publish.yml/badge.svg)](https://github.com/amantinband/clean-architecture/actions/workflows/publish.yml)

[![GitHub contributors](https://img.shields.io/github/contributors/amantinband/clean-architecture)](https://GitHub.com/amantinband/clean-architecture/graphs/contributors/) [![GitHub Stars](https://img.shields.io/github/stars/amantinband/clean-architecture.svg)](https://github.com/amantinband/clean-architecture/stargazers) [![GitHub license](https://img.shields.io/github/license/amantinband/clean-architecture)](https://github.com/amantinband/clean-architecture/blob/main/LICENSE)
[![codecov](https://codecov.io/gh/amantinband/clean-architecture/branch/main/graph/badge.svg?token=DR2EBIWK7B)](https://codecov.io/gh/amantinband/clean-architecture)

---

![Clean Architecture Template Title](assets/Clean%20Architecture%20Template%20Title.png)

---

</div>

```shell
dotnet new install Amantinband.CleanArchitecture.Template

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
  - [YouTube Tutorial](#youtube-tutorial)
  - [Install the template or clone the project](#install-the-template-or-clone-the-project)
  - [Run the service using Docker or the .NET CLI](#run-the-service-using-docker-or-the-net-cli)
  - [Generate a token](#generate-a-token)
  - [Create a subscription](#create-a-subscription)
  - [Create a reminder](#create-a-reminder)
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
- [Fun features 💃🕺](#fun-features-)
  - [Domain Events \& Eventual Consistency](#domain-events--eventual-consistency)
    - [Eventual Consistency Mechanism](#eventual-consistency-mechanism)
  - [Background service for sending email reminders](#background-service-for-sending-email-reminders)
    - [Configure Email Settings](#configure-email-settings)
    - [Configure Email Settings Manually](#configure-email-settings-manually)
    - [Configure Email Settings via User Secrets](#configure-email-settings-via-user-secrets)
- [Contribution 🤲](#contribution-)
- [Credits 🙏](#credits-)
- [License 🪪](#license-)

# ️Important notice ⚠️

This template is still under construction 👷.

Check out my comprehensive [course](https://dometrain.com/bundle/from-zero-to-hero-clean-architecture/?coupon_code=GITHUB) on Dometrain where I cover everything you need to know when building production applications structured following clean architecture. Use the exclusive coupon code `GITHUB` to get 5% off (btw this is the only promo code for a discount on the bundle, which is already 20% off).

<img src="assets/Clean%20Architecture%20Template%20Promo%20Code.png" height=30px >

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
1. Get Subscription
1. Cancel Subscription

## Reminders

1. Set Reminder
1. Get Reminder
1. Delete Reminder
1. Dismiss Reminder
1. List Reminders

# Getting Started 🏃

## YouTube Tutorial

[![Clean Architecture Tutorial](https://img.youtube.com/vi/DTmlhHu8tHA/0.jpg)](https://www.youtube.com/watch?v=DTmlhHu8tHA)

## Install the template or clone the project

```shell
dotnet new install Amantinband.CleanArchitecture.Template

dotnet new clean-arch -o CleanArchitecture
```

or

```shell
git clone https://github.com/amantinband/clean-architecture
```

## Run the service using Docker or the .NET CLI

```shell
docker compose up
```

or

```shell
dotnet run --project src/CleanArchitecture.Api
```

## Generate a token

Navigate to `requests/Tokens/GenerateToken.http` and generate a token.

> Note: Since most systems use an external identity provider, this project uses a simple token generator endpoint that generates a token based on the details you provide. This is a simple way to generate a token for testing purposes and is closer to how your system will likely be designed when using an external identity provider.

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

> ### NOTE: Replacing http file variables (`{{variableName}}`)
>
> #### Option 1 (recommended) - Using the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension for VS Code
>
> Use the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension for VS Code + update the values under [.vscode/settings.json](.vscode/settings.json). This will update the value for all http files.
>
> ```json
> {
>    "rest-client.environmentVariables": {
>        "$shared": { // these will be shared across all http files, regardless of the environment
>            "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiTGlvciIsImZhbWlseV9uYW1lIjoiRGFnYW4iLCJlbWFpbCI6Imxpb3JAZGFnYW4uY29tIiwiaWQiOiJhYWU5M2JmNS05ZTNjLTQ3YjMtYWFjZS0zMDM0NjUzYjZiYjIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsInBlcm1pc3Npb25zIjpbInNldDpyZW1pbmRlciIsImdldDpyZW1pbmRlciIsImRpc21pc3M6cmVtaW5kZXIiLCJkZWxldGU6cmVtaW5kZXIiLCJjcmVhdGU6c3Vic2NyaXB0aW9uIiwiZGVsZXRlOnN1YnNjcmlwdGlvbiIsImdldDpzdWJzY3JpcHRpb24iXSwiZXhwIjoxNzA0MTM0MTIzLCJpc3MiOiJSZW1pbmRlclNlcnZpY2UiLCJhdWQiOiJSZW1pbmRlclNlcnZpY2UifQ.wyvn9cq3ohp-JPTmbBd3G1cAU1A6COpiQd3C_e_Ng5s",
>            "userId": "aae93bf5-9e3c-47b3-aace-3034653b6bb2",
>            "subscriptionId": "c8ee11f0-d4bb-4b43-a448-d511924b520e",
>            "reminderId": "08233bb1-ce29-49e2-b346-5f8b7cf61593"
>        },
>        "dev": { // when the environment is set to dev, these values will be used
>            "host": "http://localhost:5001",
>        },
>        "prod": { // when the environment is set to prod, these values will be used
>            "host": "http://your-prod-endpoint.com",
>        }
>    }
>}
> ```
>
> #### Options 2 - Defining the variables in the http file itself
>
> Define the variables in the http file itself. This will only update the value for the current http file.
>
> ```yaml
> @host = http://localhost:5001
>
> POST {{host}}/tokens/generate
> ```
>
> #### Option 3 - Manually
>
> Replace the variables manually.
>
> ```yaml
> POST {{host}}/tokens/generate
> ```
>
> 👇
>
> ```yaml
> POST http://localhost:5001/tokens/generate
> ```
>

## Create a subscription

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

## Create a reminder

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

You can use the [this](https://www.figma.com/community/file/1334042945934670571/clean-architecture-project-file-system) figma community file to explore or create your own folder structure respresentation.

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

Will only allow users with the `get:reminder` and  `list:reminder` permission, and who pass the `SelfOrAdmin` policy, and who have the `ReminderManager` role to list reminders.

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

The application layer is tested using both unit tests and subcutaneous tests.

Since each one of the application layer use cases has its corresponding subcutaneous tests, the unit tests are used to test the application layer standalone components, such as the `ValidationBehavior` and the `AuthorizationBehavior`.

![Application Layer unit tests](assets/Clean%20Architecture%20Template%20Application%20Layer%20Unit%20Tests.png)

### Application Layer Subcutaneous Tests

Subcutaneous tests are tests that operate right under the presentation layer.
These tests are responsible for testing the core logic of our application, which is the application layer and the domain layer.

The reason there are so many of these tests, is because each one of the application layer use cases has its corresponding subcutaneous tests.

This allows us to test the application layer and the domain layer based on the actual expected usage, giving us the confidence that our application works as expected and that the system cannot be manipulated in a way we don't allow.

I recommend spending more effort on these tests than the other tests, since they aren't too expensive to write, and the value they provide is huge.

![](assets/Clean%20Architecture%20Template%20Subcutaneous%20Tests.png)

### Presentation Layer Integration Tests

The api layer is tested using integration tests. This is where we want to cover the entire system, including the database, external dependencies and the presentation layer.

Unlike the subcutaneous tests, the focus of these tests is to ensure the integration between the various components of our system and other systems.

![Integration Tests](assets/Clean%20Architecture%20Template%20Integration%20Tests.png)

# Fun features 💃🕺

## Domain Events & Eventual Consistency

> Note: Eventual consistency and the domain events pattern add a layer of complexity. If you don't need it, don't use it. If you need it, make sure your system is designed properly and that you have the right tools to manage failures.

The domain is designed so each use case which manipulates data, updates a single domain object in a single transaction.

For example, when a user cancels a subscription, the only change that happens atomically is the subscription is marked as canceled:

```csharp
public ErrorOr<Success> CancelSubscription(Guid subscriptionId)
{
    if (subscriptionId != Subscription.Id)
    {
        return Error.NotFound("Subscription not found");
    }

    Subscription = Subscription.Canceled;

    _domainEvents.Add(new SubscriptionCanceledEvent(this, subscriptionId));

    return Result.Success;
}
```

Then, in an eventual consistency manner, the system will update all the relevant data. Which includes:

1. Deleting the subscription from the database and marking all reminders as deleted ([Subscriptions/Events/SubscriptionDeletedEventHandler.cs](src/CleanArchitecture.Application/Subscriptions/Events/SubscriptionCanceledEventHandler.cs)])
1. Deleting all the reminders marked as deleted from the database ([Reminders/Events/ReminderDeletedEventHandler.cs](src/CleanArchitecture.Application/Reminders/Events/ReminderDeletedEventHandler.cs)]

> Note: Alongside the performance benefits, this allows to reuse reactive behavior. For example, the `ReminderDeletedEventHandler` is invoked both when a subscription is deleted and when a reminder is deleted.

### Eventual Consistency Mechanism

1. Each invariant is encapsulated in a single domain object. This allows performing changes by updating a single domain object in a single transaction.
1. If `domain object B` needs to react to changes in `domain object A`, a [Domain Event](src/CleanArchitecture.Domain/Common/IDomainEvent.cs) is added to `domain object A` alongside the changes.
1. Upon persisting `domain object A` changes to the database, the domain events are [extracted and added to a queue](src/CleanArchitecture.Infrastructure/Common/Persistence/AppDbContext.cs) for offline processing:
    ```csharp
    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        Queue<IDomainEvent> domainEventsQueue = new();
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        _httpContextAccessor.HttpContext.Items["DomainEvents"] = domainEventsQueue;
    }
    ```
1. After the user receives a response, the [EventualConsistencyMiddleware](src/CleanArchitecture.Infrastructure/Common/Middleware/EventualConsistencyMiddleware.cs) is invoked and processes the domain events:
    ```csharp
    public async Task InvokeAsync(HttpContext context, IEventualConsistencyProcessor eventualConsistencyProcessor)
    {
        context.Response.OnCompleted(async () =>
        {
            if (context.Items.TryGetValue("DomainEvents", out var value) ||
                value is not Queue<IDomainEvent> domainEvents)
            {
                return;
            }

            while (domainEvents.TryDequeue(out var nextEvent))
            {
                await publisher.Publish(nextEvent);
            }
        });
    }
    ```

> Note: the code snippets above are a simplified version of the actual implementation.

## Background service for sending email reminders

There is a simple background service that runs every minute and sends email reminders for all reminders that are due ([ReminderEmailBackgroundService](src/CleanArchitecture.Infrastructure/Reminders/BackgroundServices/ReminderEmailBackgroundService.cs)):

```csharp
private async void SendEmailNotifications(object? state)
{
    await _fluentEmail
        .To(user.Email)
        .Subject($"{dueReminders.Count} reminders due!")
        .Body($"""
              Dear {user.FirstName} {user.LastName} from the present.

              I hope this email finds you well.

              I'm writing you this email to remind you about the following reminders:
              {string.Join('\n', dueReminders.Select((reminder, i) => $"{i + 1}. {reminder.Text}"))}

              Best,
              {user.FirstName} from the past.
              """)
        .SendAsync();
}
```

### Configure Email Settings

To configure the service to send emails, make sure to update the email settings under the `appsettings.json`/`appsettings.Development.json` file:

You can use your own SMTP server or use a service like [Brevo](https://brevo.com/).

### Configure Email Settings Manually

```json
{
  "EmailSettings": {
    "EnableEmailNotifications": false,
    "DefaultFromEmail": "your-email@gmail.com (also, change EnableEmailNotifications to true 👆)",
    "SmtpSettings": {
      "Server": "smtp.gmail.com",
      "Port": 587,
      "Username": "your-email@gmail.com",
      "Password": "your-password"
    }
  }
}
```

> note: you may need to allow less secure apps to access your email account.

### Configure Email Settings via User Secrets

```shell
dotnet user-secrets --project src/CleanArchitecture.Api set EmailSettings:EnableEmailNotifications true
dotnet user-secrets --project src/CleanArchitecture.Api set EmailSettings:DefaultFromEmail amantinband@gmail.com
dotnet user-secrets --project src/CleanArchitecture.Api set EmailSettings:SmtpSettings:Server smtp-relay.brevo.com
dotnet user-secrets --project src/CleanArchitecture.Api set EmailSettings:SmtpSettings:Port 587
dotnet user-secrets --project src/CleanArchitecture.Api set EmailSettings:SmtpSettings:Username amantinband@gmail.com
dotnet user-secrets --project src/CleanArchitecture.Api set EmailSettings:SmtpSettings:Password your-password
```

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# Credits 🙏

- [CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) - An awesome clean architecture solution template by Jason Taylor

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/mantinband/clean-architecture/blob/main/LICENSE) license.
