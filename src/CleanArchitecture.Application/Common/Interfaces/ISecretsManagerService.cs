namespace CleanArchitecture.Application.Common.Interfaces;

public interface ISecretsManagerService
{
    Task<string?> GetAsync(string key);

    Task CreateAsync(string key, string value);

    Task CreateAsync<T>(string key, T value)
        where T : class;
}
