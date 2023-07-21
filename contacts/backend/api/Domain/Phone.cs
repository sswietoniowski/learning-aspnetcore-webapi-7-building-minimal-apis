using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Contacts.Api.Domain;

public class Phone
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(16)]
    public required string Number { get; set; }
    public string Description { get; set; } = string.Empty;
    public Contact Contact { get; set; } = default!;
    [Required]
    [ForeignKey(nameof(Contact))]
    public int ContactId { get; set; }

    public Phone() { }

    [SetsRequiredMembers]
    public Phone(int id, string number)
    {
        Id = id;
        Number = number;
    }
}