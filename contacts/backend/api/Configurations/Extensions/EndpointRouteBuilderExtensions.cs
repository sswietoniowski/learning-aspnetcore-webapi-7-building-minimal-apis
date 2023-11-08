using Contacts.Api.Configurations.EndpointHandlers;

namespace Contacts.Api.Configurations.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterContactsEndpoints(this IEndpointRouteBuilder app)
    {
        var contactsEndpoints = app.MapGroup("/api/contacts");

        // GET api/contacts
        // GET api/contacts?lastName=Nowak
        // GET api/contacts?search=ski
        // GET api/contacts?search=ski&orderBy=LastName&desc=true
        contactsEndpoints.MapGet("", ContactsHandlers.GetContactsAsync);

        // GET api/contacts/1
        contactsEndpoints.MapGet("{id:int}", ContactsHandlers.GetContactAsync).WithName("GetContact");

        // POST api/contacts
        contactsEndpoints.MapPost("", ContactsHandlers.CreateContactAsync);

        // PUT api/contacts/1
        contactsEndpoints.MapPut("{id:int}", ContactsHandlers.UpdateContactAsync)
            // Add a filter to the endpoint that will prevent updating read only contacts
            .AddEndpointFilter(async (context, next) => 
            {
                var contactId = context.GetArgument<int>(0);
                var readOnlyContactIds = new[] { 1  };

                if (readOnlyContactIds.Contains(contactId))
                {

                    return TypedResults.Problem(new ()
                    {
                        Status = 400,
                        Title = "Contact is read only and cannot be changed.",
                        Detail = $"Contact with id {contactId} is read only and cannot be changed."
                    });
                }

                // invoke the next filter
                var result = await next.Invoke(context);

                return result;
            });

        // DELETE api/contacts/1
        contactsEndpoints.MapDelete("{id:int}", ContactsHandlers.DeleteContactAsync);
    }

    public static void RegisterPhonesEndpoints(this IEndpointRouteBuilder app)
    {
        var phonesEndpoints = app.MapGroup("/api/contacts/{contactId:int}/phones");

        // GET api/contacts/1/phones
        phonesEndpoints.MapGet("", PhonesHandlers.GetPhones);

        // GET api/contacts/1/phones/1
        phonesEndpoints.MapGet("{phoneId:int}", PhonesHandlers.GetPhone);
    }
}