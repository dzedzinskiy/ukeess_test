using DAL.Factories;

namespace DAL.Models.Contacts
{
    public class SecondaryContact : Contact
    {
        public SecondaryContact()
        {
        }

        public SecondaryContact(Contact contact) : base(contact)
        {
        }
    }

    public class SecondaryContactCreator : ContactCreator
    {
        public override Contact FactoryMethod()
        {
            return new SecondaryContact();
        }

        public override Contact FactoryMethod(Contact contact)
        {
            return new SecondaryContact(contact);
        }
    }
}