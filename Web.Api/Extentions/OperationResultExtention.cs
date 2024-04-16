using Domain.Core.Primitives.Result;

namespace Web.Api.Extentions;

public static class OperationResultExtention
{
    public static IResult ToBadRequestProblem(this Result result)
    {
        if (result.Error is null)
            throw new InvalidOperationException();

        return Results.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error} }
            });
    }

    public static IResult ToNotFoundProblem(this Result result)
    {
        if (result.Error is null)
            throw new InvalidOperationException();

        return Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error} }
            });
    }
}
