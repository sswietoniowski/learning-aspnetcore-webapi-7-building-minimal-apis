using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public class ContactForUpdateDto
{
    [Required]
    [MaxLength(32)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(64)]
    public string LastName { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}