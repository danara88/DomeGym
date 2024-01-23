using ErrorOr;
using FluentValidation;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

/// <summary>
/// Generic pipeline behavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse>
  : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
      where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        // Convert errors to error or errors
        var errors = validationResult.Errors
            .ConvertAll(error =>
                Error.Validation(code: error.PropertyName, description: error.ErrorMessage));

        return (dynamic)errors;
    }
}
