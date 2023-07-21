namespace Contacts.Api.DTOs;

/// <summary>
/// The contact's phone
/// </summary>
public class PhoneDto
{
    /// <summary>
    /// The id of the phone
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The number of the phone
    /// </summary>
    public string Number { get; set; } = string.Empty;
    /// <summary>
    /// The description of the phone
    /// </summary>
    public string Description { get; set; } = string.Empty;
}