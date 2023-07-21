using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

/// <summary>
/// The contact for creation
/// </summary>
public class ContactForCreationDto
{
    /// <summary>
    /// The first name of the contact (must be different from the last name)
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// The last name of the contact (must be different from the first name)
    /// </summary>
    [Required]
    [StringLength(64)]
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// The email of the contact
    /// </summary>
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}