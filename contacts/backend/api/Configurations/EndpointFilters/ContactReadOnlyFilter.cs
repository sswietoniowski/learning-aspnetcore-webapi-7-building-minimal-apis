namespace Contacts.Api.Configurations.EndpointFilters;

public class ContactReadOnlyFilter : IEndpointFilter
{
    private readonly int _readOnlyContactId;

    public ContactReadOnlyFilter(int readOnlyContactId)
    {
        _readOnlyContactId = readOnlyContactId;
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int contactId;

        if (context.HttpContext.Request.Method == "PUT")
        {
            contactId = context.GetArgument<int>(0);
        }
        else if (context.HttpContext.Request.Method == "DELETE")
        {
            contactId = context.GetArgument<int>(0);
        }
        else
        {
            throw new NotSupportedException("This filter is not supported for this scenario.");
        }

        if (contactId == _readOnlyContactId)
        {
            return new ValueTask<object?>(TypedResults.Problem(new()
            {
                Status = 400,
                Title = "Contact is read only and cannot be changed.",
                Detail = $"Contact with id {contactId} is read only and cannot be changed."
            }));
        }

        // invoke the next filter
        var result = next(context);
        return result;
    }
}