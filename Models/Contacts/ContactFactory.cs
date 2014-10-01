namespace Models.Contacts
{
    public interface ICreator
    {
        Contact FactoryMethod();
    }

    public abstract class ContactCreator : ICreator
    {
        public abstract Contact FactoryMethod();
        public abstract Contact FactoryMethod(Contact contact);
    }
}