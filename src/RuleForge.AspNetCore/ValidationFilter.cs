using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RuleForge.AspNetCore
{
    /// <summary>
    /// Action filter that performs validation using RuleForge validators.
    /// </summary>
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (KeyValuePair<string, object?> argument in context.ActionArguments)
            {
                if (argument.Value is null)
                {
                    continue;
                }

                Type argumentType = argument.Value.GetType();
                Type validatorType = typeof(Validator<>).MakeGenericType(argumentType);

                object? validator = context.HttpContext.RequestServices.GetService(validatorType);
                if (validator is null)
                {
                    continue;
                }

                try
                {
                    dynamic dynamicValidator = validator;
                    ValidationResult validationResult = await dynamicValidator.ValidateAsync((dynamic)argument.Value);

                    if (!validationResult.IsValid)
                    {
                        ValidationProblemDetails problemDetails = new ValidationProblemDetails
                        {
                            Title = "Validation Error",
                            Status = StatusCodes.Status400BadRequest
                        };

                        foreach (ValidationError error in validationResult.Errors)
                        {
                            if (!problemDetails.Errors.ContainsKey(error.PropertyName))
                            {
                                problemDetails.Errors[error.PropertyName] = new string[] { error.ErrorMessage };
                            }
                            else
                            {
                                List<string> messages = problemDetails.Errors[error.PropertyName].ToList();
                                messages.Add(error.ErrorMessage);
                                problemDetails.Errors[error.PropertyName] = messages.ToArray();
                            }
                        }

                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception if needed
                    continue;
                }
            }

            await next();
        }
    }
}