using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;
using Models.Contacts;

namespace DAL.Repositories
{
    public interface IUserRepository<TEntity, TKey> : IDisposable where TEntity : class
    {
        void Create(TEntity entity);
        TEntity GetById(TKey id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate, string includes);


        IEnumerable<User> GetAllUsers();
        User GetUserWithContactsById(int userId);
        void DeleteUserWithContacts(int userId);

        void InsertUserContact(int userId, Contact contact);
        void DeleteUserContact(int userId, int contactId, int contactType);
        void UpdateUserContact(Contact contact);

        
    }
}