using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public class ContactWithPhonesForCreationDto : ContactForCreationDto
{
    [Required]
    public List<PhoneForCreationDto> Phones { get; set; } = new();
}