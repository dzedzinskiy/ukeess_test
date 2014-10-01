using System.Collections.Generic;
using DAL.Repositories;
using Models;
using Models.Contacts;
using Models.DataContexts;

namespace BLL.DataAccess
{
    public class UserRepositoryProxy : IUserRepositoryProxy
    {
        private readonly IAppContext context;
        private bool disposed;
        private readonly IUserRepository repository;

        public UserRepositoryProxy(IAppContext appContext)
        {
            this.context = appContext;
            this.repository = new UserRepository(context);
        }

        public User GetUserById(int id)
        {
            return repository.GetUserWithContactsById(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return repository.GetAllUsers();
        }

        public void InsertUser(User user)
        {
            repository.InsertUser(user);
        }

        public void UpdateUser(User user)
        {
            repository.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            repository.DeleteUser(id);
        }

        public void InsertUserContact(int userId, Contact contact)
        {
            repository.InsertUserContact(userId, contact);
        }

        public void DeleteUserContact(int userId, int contactId, int contactType)
        {
            repository.DeleteUserContact(userId, contactId, contactType);
        }

        public void UpdateUserContact(Contact contact)
        {
            repository.UpdateUserContact(contact);
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}