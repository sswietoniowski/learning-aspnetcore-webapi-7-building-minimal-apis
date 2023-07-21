using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Api.Domain;

public class Phone
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(16)]
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Contact Contact { get; set; } = default!;
    [Required]
    [ForeignKey(nameof(Contact))]
    public int ContactId { get; set; }
}