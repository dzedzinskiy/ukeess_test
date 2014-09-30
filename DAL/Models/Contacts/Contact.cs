using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Contacts
{
    public enum ContactTypes
    {
        Primary,
        Secondary,
        Administrative
    }

    public class Contact : IContact
    {
        public Contact()
        {
        }

        public Contact(Contact contact)
        {
            ID = contact.ID;
            Name = contact.Name;
            Phone = contact.Phone;
            Fax = contact.Fax;
            Email = contact.Email;
            Note = contact.Note;
            ContactType = contact.ContactType;
        }

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        [NotMapped]
        public ContactTypes ContactType { get; set; }
    }
}