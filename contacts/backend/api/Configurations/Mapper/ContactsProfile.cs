using AutoMapper;
using Contacts.Api.Domain;
using Contacts.Api.DTOs;

namespace Contacts.Api.Configurations.Mapper;

public class ContactsProfile : Profile
{
    public ContactsProfile()
    {
        CreateMap<Contact, ContactDto>();
        CreateMap<Contact, ContactDetailsDto>();
        CreateMap<Phone, PhoneDto>();
        CreateMap<ContactForCreationDto, Contact>();
        CreateMap<ContactWithPhonesForCreationDto, Contact>();
        CreateMap<PhoneForCreationDto, Phone>();
        CreateMap<ContactForUpdateDto, Contact>().ReverseMap();
    }
}