namespace Models.Contacts
{
    public class AdministrativeContact : Contact
    {
        public AdministrativeContact()
        {
        }

        public AdministrativeContact(Contact contact) : base(contact)
        {
        }


        public class AdministrativeContactCreator : ContactCreator
        {
            public override Contact FactoryMethod()
            {
                return new AdministrativeContact();
            }

            public override Contact FactoryMethod(Contact contact)
            {
                return new AdministrativeContact(contact);
            }
        }
    }
}