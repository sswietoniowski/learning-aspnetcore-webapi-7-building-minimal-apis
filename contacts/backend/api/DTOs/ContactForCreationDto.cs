using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record ContactForCreationDto(
    [Required][MaxLength(32)] string FirstName,
    [Required][StringLength(64)] string LastName,
    [EmailAddress] string Email);
