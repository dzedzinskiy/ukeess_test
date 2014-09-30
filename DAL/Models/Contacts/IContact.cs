namespace DAL.Models.Contacts
{
    public interface IContact
    {
        string Name { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        string Email { get; set; }
        string Note { get; set; }

        ContactTypes ContactType { get; set; }
    }
}