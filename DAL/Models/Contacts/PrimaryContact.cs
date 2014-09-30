using System.Text;
using DAL.Factories;

namespace DAL.Models.Contacts
{
    public class PrimaryContact : Contact
    {
        public PrimaryContact()
        {
        }

        public PrimaryContact(Contact contact) : base(contact)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} : {1}\n {2} : {3}\n {4} : {5}",
                "Contact name", Name,
                "Email", Email,
                "Phone", Phone);
            return sb.ToString();
        }
    }

    public class PrimaryContactCreator : ContactCreator
    {
        public override Contact FactoryMethod()
        {
            return new PrimaryContact();
        }

        public override Contact FactoryMethod(Contact contact)
        {
            return new PrimaryContact(contact);
        }
    }
}