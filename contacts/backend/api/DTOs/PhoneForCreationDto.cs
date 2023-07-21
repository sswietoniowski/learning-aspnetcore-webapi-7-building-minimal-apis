using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record PhoneForCreationDto(
    [Required][MaxLength(16)] string Number,
    string Description);
