using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record ContactWithPhonesForCreationDto(string FirstName, string LastName, string Email,
    List<PhoneForCreationDto> Phones)
    : ContactForCreationDto(FirstName, LastName, Email);
