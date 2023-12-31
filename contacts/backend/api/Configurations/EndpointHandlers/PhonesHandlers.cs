﻿using Contacts.Api.DTOs;
using Contacts.Api.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Configurations.EndpointHandlers;

public static class PhonesHandlers
{
    public static Results<Ok<IEnumerable<PhoneDto>>, NotFound> GetPhones([FromRoute] int contactId,
        [FromServices] ContactsDbContext dbContext, ILogger<PhoneDto> logger)
    {
        logger.LogInformation("Getting phones for contact with id {contactId}", contactId);

        var contact = dbContext.Contacts.Include(c => c.Phones)
            .FirstOrDefault(c => c.Id == contactId);

        if (contact is null)
        {
            return TypedResults.NotFound();
        }

        var phonesDto = contact.Phones
            .Select(p => new PhoneDto(p.Id, p.Number, p.Description));

        return TypedResults.Ok(phonesDto);
    }

    public static Results<Ok<PhoneDto>, NotFound> GetPhone([FromRoute] int contactId, [FromRoute] int phoneId,
        [FromServices] ContactsDbContext dbContext, ILogger<PhoneDto> logger)
    {
        logger.LogInformation("Getting phone with id {phoneId} for contact with id {contactId}", phoneId, contactId);

        var contact = dbContext.Contacts.Include(c => c.Phones)
            .FirstOrDefault(c => c.Id == contactId);

        if (contact is null)
        {
            return TypedResults.NotFound();
        }

        var phone = contact.Phones.FirstOrDefault(p => p.Id == phoneId);

        if (phone is null)
        {
            return TypedResults.NotFound();
        }

        var phoneDto = new PhoneDto(phone.Id, phone.Number, phone.Description);

        return TypedResults.Ok(phoneDto);
    }

}