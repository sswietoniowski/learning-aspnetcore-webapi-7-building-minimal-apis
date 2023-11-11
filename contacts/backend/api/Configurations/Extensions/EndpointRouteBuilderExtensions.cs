using Contacts.Api.Configurations.EndpointFilters;
using Contacts.Api.Configurations.EndpointHandlers;

namespace Contacts.Api.Configurations.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterContactsEndpoints(this IEndpointRouteBuilder app)
    {
        var contactsEndpoints = app.MapGroup("/api/contacts");

        var contactsEndpointsForChange = app.MapGroup("/api/contacts")
            .AddEndpointFilter(new ContactReadOnlyFilter(2))
            .AddEndpointFilter(new ContactReadOnlyFilter(3));

        // GET api/contacts
        // GET api/contacts?lastName=Nowak
        // GET api/contacts?search=ski
        // GET api/contacts?search=ski&orderBy=LastName&desc=true
        contactsEndpoints.MapGet("", ContactsHandlers.GetContactsAsync);

        // GET api/contacts/1
        contactsEndpoints.MapGet("{id:int}", ContactsHandlers.GetContactAsync).WithName("GetContact");

        // POST api/contacts
        contactsEndpoints.MapPost("", ContactsHandlers.CreateContactAsync)
            .AddEndpointFilter<ValidateAnnotationsFilter>();

        // PUT api/contacts/1
        contactsEndpointsForChange.MapPut("{id:int}", ContactsHandlers.UpdateContactAsync);

        // DELETE api/contacts/1
        contactsEndpointsForChange.MapDelete("{id:int}", ContactsHandlers.DeleteContactAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();
    }

    public static void RegisterPhonesEndpoints(this IEndpointRouteBuilder app)
    {
        var phonesEndpoints = app.MapGroup("/api/contacts/{contactId:int}/phones");

        // GET api/contacts/1/phones
        phonesEndpoints.MapGet("", PhonesHandlers.GetPhones)
            .RequireAuthorization();

        // GET api/contacts/1/phones/1
        phonesEndpoints.MapGet("{phoneId:int}", PhonesHandlers.GetPhone)
            .RequireAuthorization("RequireAdminFromPoland");
    }
}