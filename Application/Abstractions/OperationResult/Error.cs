namespace Application.Abstractions.OperationResult;

public record Error(string Title, string? Description = null);
