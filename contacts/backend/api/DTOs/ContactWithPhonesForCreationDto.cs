using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record ContactWithPhonesForCreationDto(
        [Required][MaxLength(32)] string FirstName,
        [Required][StringLength(64)] string LastName,
        [EmailAddress] string Email,
        List<PhoneForCreationDto> Phones)
    : ContactForCreationDto(FirstName, LastName, Email);
