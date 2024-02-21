namespace Application.Abstractions.OperationResult.Base;

public record Error(string Title, string? Description = null);
