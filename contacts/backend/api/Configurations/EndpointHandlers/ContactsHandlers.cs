using AutoMapper;
using Contacts.Api.DTOs;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contacts.Api.Domain;

namespace Contacts.Api.Configurations.EndpointHandlers;

public static class ContactsHandlers
{
    // ReSharper disable once InconsistentNaming
    const int DefaultContactsPageNumber = 1;
    // ReSharper disable once InconsistentNaming
    const int DefaultContactsPageSize = 10;
    // ReSharper disable once InconsistentNaming
    const int MaxContactsPageSize = 50;

    public static async Task<Results<Ok<IEnumerable<ContactDto>>, BadRequest>> GetContactsAsync([FromQuery] string? lastName, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] bool? desc,
        int? pageNumber, int? pageSize,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper, HttpContext context)
    {
        pageNumber ??= DefaultContactsPageNumber;

        pageSize ??= DefaultContactsPageSize;

        if (pageNumber <= 0)
        {
            return TypedResults.BadRequest();
        }

        if (pageSize > MaxContactsPageSize)
        {
            pageSize = MaxContactsPageSize;
        }

        var (contacts, paginationMetadata) = await repository.GetContactsAsync(lastName, search, orderBy, desc, (int)pageNumber, (int)pageSize);

        var contactsDto = mapper.Map<IEnumerable<ContactDto>>(contacts);

        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(contactsDto);
    }

    public static async Task<Results<Ok<ContactDto>, NotFound>> GetContactAsync([FromRoute] int id,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper)
    {
        var contact = await repository.GetContactAsync(id);

        if (contact is null)
        {
            return TypedResults.NotFound();
        }

        var contactDto = mapper.Map<ContactDto>(contact);

        return TypedResults.Ok(contactDto);
    }

    public static async Task<CreatedAtRoute<ContactDto>> CreateContactAsync([FromBody] ContactForCreationDto contactForCreationDto,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper)
    {
        var contact = mapper.Map<Contact>(contactForCreationDto);

        await repository.CreateContactAsync(contact);

        var contactDto = mapper.Map<ContactDto>(contact);

        return TypedResults.CreatedAtRoute(contactDto, "GetContact", new { id = contactDto.Id });
    }

    public static async Task<Results<NoContent, NotFound>> UpdateContactAsync([FromRoute] int id, [FromBody] ContactForUpdateDto contactForUpdateDto,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper)
    {
        var contact = mapper.Map<Contact>(contactForUpdateDto);
        contact.Id = id;

        var success = await repository.UpdateContactAsync(contact);

        if (!success)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteContactAsync([FromRoute] int id,
        [FromServices] IContactsRepository repository)
    {
        var success = await repository.DeleteContactAsync(id);

        if (!success)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }
}