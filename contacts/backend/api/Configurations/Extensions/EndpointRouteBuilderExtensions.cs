using Contacts.Api.Configurations.EndpointFilters;
using Contacts.Api.Configurations.EndpointHandlers;
using Contacts.Api.DTOs;

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
        contactsEndpoints.MapGet("", ContactsHandlers.GetContactsAsync)
            .WithName("GetContacts")
            .WithOpenApi()
            .WithSummary("Gets all contacts")
            .WithDescription("Gets all contacts from the database")
            .Produces<IEnumerable<ContactDto>>();

        // GET api/contacts/1
        contactsEndpoints.MapGet("{id:int}", ContactsHandlers.GetContactByIdAsync).WithName("GetContact");

        // POST api/contacts
        contactsEndpoints.MapPost("", ContactsHandlers.CreateContactAsync)
            .AddEndpointFilter<ValidateAnnotationsFilter>()
            .WithName("CreateContact")
            .WithOpenApi()
            .WithSummary("Creates a new contact")
            .WithDescription("Creates a new contact in the database")
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Accepts<ContactForCreationDto>("application/json");

        // PUT api/contacts/1
        contactsEndpointsForChange.MapPut("{id:int}", ContactsHandlers.UpdateContactAsync);

        // DELETE api/contacts/1
        contactsEndpoints.MapDelete("{id:int}", ContactsHandlers.DeleteContactAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>()
            .WithName("DeleteContact")
            .WithOpenApi()
            .WithSummary("Deletes a contact by id")
            .WithDescription("Deletes a contact by id from the database")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation =>
            {
                operation.Deprecated = true;
                return operation;
            });
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