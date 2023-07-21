using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contacts.Api.Domain;

public class Contact
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(32)]
    public required string FirstName { get; set; }
    [Required]
    [StringLength(64)]
    public required string LastName { get; set; }
    [MaxLength(128)]
    public required string Email { get; set; }
    public List<Phone> Phones { get; set; } = new();

    public Contact() {}

    [SetsRequiredMembers]
    public Contact(int id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}