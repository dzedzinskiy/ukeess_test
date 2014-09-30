using DAL.Models.Contacts;

namespace DAL.Factories
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