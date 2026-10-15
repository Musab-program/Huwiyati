namespace Huwiyati.API.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using Huwiyati.Application.Common;

/// <summary>
/// Action filter that automatically intercepts incoming action arguments, runs FluentValidation rules, and produces a structured <see cref="ApiResponse{T}"/> 400 Bad Request if validation fails.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    /// <summary>
    /// Executes the action filter asynchronously, inspecting arguments against registered validators.
    /// </summary>
    /// <param name="context">The action executing context.</param>
    /// <param name="next">The delegate representing the remainder of the action execution pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous filter execution.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var serviceProvider = context.HttpContext.RequestServices;

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var validator = serviceProvider.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .Distinct()
                        .ToList();

                    var response = ApiResponse<object>.Failure(
                        message: "Validation failed. Please correct the highlighted errors.",
                        errors: errors,
                        statusCode: StatusCodes.Status400BadRequest);

                    context.Result = new BadRequestObjectResult(response);
                    return;
                }
            }
        }

        await next();
    }
}
