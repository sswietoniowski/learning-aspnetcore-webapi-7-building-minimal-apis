namespace Contacts.Api.DTOs;

/// <summary>
/// The contact details
/// </summary>
public class ContactDetailsDto
{
    /// <summary>
    /// The id of the contact
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The first name of the contact
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// The last name of the contact
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// The full name of the contact
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
    /// <summary>
    /// The email of the contact
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// The phones of the contact
    /// </summary>
    public List<PhoneDto> Phones { get; set; } = new();
}