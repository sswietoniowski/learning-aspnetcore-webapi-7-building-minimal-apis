using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public class PhoneForCreationDto
{
    [Required]
    [MaxLength(16)]
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}