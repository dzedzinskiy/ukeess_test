using DAL.Models.Contacts;
using DAL.Repositories;

namespace DAL
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        GenericRepository<Contact> ContactRepository { get; }
        void Save();
        void Dispose();
    }
}