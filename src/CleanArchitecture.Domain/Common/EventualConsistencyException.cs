using ErrorOr;

namespace CleanArchitecture.Domain.Common;

public class EventualConsistencyException(
    Error eventualConsistencyError,
    List<Error>? underlyingErrors = null)
        : Exception(message: eventualConsistencyError.Description)
{
    public Error EventualConsistencyError { get; } = eventualConsistencyError;
    public List<Error> UnderlyingErrors { get; } = underlyingErrors ?? [];
}