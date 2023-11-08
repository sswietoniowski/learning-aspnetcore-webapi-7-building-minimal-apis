using Contacts.Api.DTOs;
using MiniValidation;

namespace Contacts.Api.Configurations.EndpointFilters;

public class ValidateAnnotationsFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var contactForCreationDto = context.GetArgument<ContactForCreationDto>(0);

        if (!MiniValidator.TryValidate(contactForCreationDto, out var validationErrors))
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        return await next(context);
    }
}