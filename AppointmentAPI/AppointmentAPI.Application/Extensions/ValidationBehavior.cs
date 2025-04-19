using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using MediatR;

namespace AppointmentAPI.Application.Extensions;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Count() == 0)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var errors = _validators.Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (errors.Count != 0)
        {
            string[] errorMessages;
            errorMessages = errors.Select(e => e.ErrorMessage).ToArray();

            throw new ValidationAppException(errorMessages);
        }

        return await next();
    }
}
