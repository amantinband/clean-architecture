using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitecture.Application.Common.Interfaces;

using Infisical.Sdk;
using Infisical.Sdk.Model;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure.Security.Services;

public class SecretsManagerService : ISecretsManagerService
{
    private readonly InfisicalSdkSettings _infisicalSdkSettings = new InfisicalSdkSettingsBuilder()
        .WithHostUri("localhost:7030")
        .Build();

    private readonly ListSecretsOptions _listSecretsOptions;
    private readonly InfisicalClient _infisicalClient;
    private readonly ILogger<SecretsManagerService> _logger;
    public SecretsManagerService(ILogger<SecretsManagerService> logger)
    {
        _infisicalClient = new InfisicalClient(_infisicalSdkSettings);
        _infisicalClient.Auth().UniversalAuth().LoginAsync("client-id", "client-secret"); // TODO: Replace with your actual client ID and secret

        _listSecretsOptions = new ListSecretsOptions
        {
            SetSecretsAsEnvironmentVariables = true,
            EnvironmentSlug = "development", // TODO: Replace with your actual environment slug
            SecretPath = "/",
            ProjectId = "1f584746-bfea-4588-b7f1-b6351223a807", // TODO: Replace with your actual project ID
        };

        _logger = logger;
    }

    public async Task<string?> GetAsync(string key)
    {
        var options = new GetSecretOptions
        {
            SecretName = key,
            SecretPath = "/",
            EnvironmentSlug = _listSecretsOptions.EnvironmentSlug,
            ProjectId = _listSecretsOptions.ProjectId,
        };

        Secret secret = await _infisicalClient.Secrets().GetAsync(options);
        if (secret == null)
        {
            return null;
        }

        return secret.SecretValue;
    }

    public async Task CreateAsync(string key, string value)
    {
        var options = new CreateSecretOptions
        {
            SecretName = key,
            SecretValue = value ?? string.Empty,
            SecretPath = "/",
            EnvironmentSlug = _listSecretsOptions.EnvironmentSlug,
            ProjectId = _listSecretsOptions.ProjectId,
        };
        await _infisicalClient.Secrets().CreateAsync(options);
    }

    public async Task CreateAsync<T>(string key, T value)
        where T : class
    {
        var serializer = JsonSerializer.CreateDefault();
        var jsonSerializedValue = JsonConvert.SerializeObject(value, serializer.Formatting, serializer.Converters.ToArray());

        var options = new CreateSecretOptions
        {
            SecretName = key,
            SecretValue = jsonSerializedValue ?? string.Empty,
            SecretPath = "/",
            EnvironmentSlug = _listSecretsOptions.EnvironmentSlug,
            ProjectId = _listSecretsOptions.ProjectId,
        };
        await _infisicalClient.Secrets().CreateAsync(options);
    }

    public async Task UpdateAsync<T>(string key, T value)
    {
        var serializer = JsonSerializer.CreateDefault();
        var jsonSerializedValue = JsonConvert.SerializeObject(value, serializer.Formatting, serializer.Converters.ToArray());

        var options = new UpdateSecretOptions
        {
            SecretName = key,
            NewSecretValue = jsonSerializedValue ?? string.Empty,
            SecretPath = "/",
            EnvironmentSlug = _listSecretsOptions.EnvironmentSlug,
            ProjectId = _listSecretsOptions.ProjectId,
        };
        await _infisicalClient.Secrets().UpdateAsync(options);
    }

    public async Task BulkUpdateAsync(Dictionary<string, string> pairs)
    {
        //var options = new UpdateSecretOptions
        //{
        //    SecretName = key,
        //    NewSecretValue = jsonSerializedValue ?? string.Empty,
        //    SecretPath = "/",
        //    EnvironmentSlug = _listSecretsOptions.EnvironmentSlug,
        //    ProjectId = _listSecretsOptions.ProjectId,
        //};
        //await _infisicalClient.Secrets().UpdateAsync(options);
    }

    public async Task DeleteAsync(string key)
    {
        var options = new DeleteSecretOptions
        {
            SecretName = key,
            SecretPath = "/",
            EnvironmentSlug = _listSecretsOptions.EnvironmentSlug,
            ProjectId = _listSecretsOptions.ProjectId,
        };
        await _infisicalClient.Secrets().DeleteAsync(options);
        _logger.LogInformation($"Secret with key '{key}' has been deleted successfully from Infisical.");
    }
}
