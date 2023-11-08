using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record ContactForCreationDto([Required] string FirstName, [Required] string LastName, [Required][EmailAddress] string Email);
