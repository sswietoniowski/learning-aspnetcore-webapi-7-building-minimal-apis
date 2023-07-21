using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Domain;

public class Contact
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(64)]
    public string LastName { get; set; } = string.Empty;
    [MaxLength(128)]
    public string Email { get; set; } = string.Empty;
    public List<Phone> Phones { get; set; } = new();
}