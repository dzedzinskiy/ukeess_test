using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Models;
using Models.Contacts;
using Models.DataContexts;

namespace DAL.Repositories
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        private IAppContext context { get; set; }

        public UserRepository(IAppContext context) : base((DbContext) context)
        {
            this.context = context;
        }

        private bool disposed;

        public User GetUserById(int id)
        {
            User user = context.Users
                .Include("PrimaryContact")
                .Include("SecondaryContact")
                .Include("AdministrativeContact").FirstOrDefault(_ => _.ID == id);
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return context.Users
                .Include("PrimaryContact")
                .Include("SecondaryContact")
                .Include("AdministrativeContact")
                .ToList();
        }

        public void InsertUser(User user)
        {
            base.Create(user);
            /*context.Users.Add(user);*/
        }

        public void UpdateUser(User user)
        {
            User userFromDb = context.Users.Single(_ => _.ID == user.ID);
            context.Entry(userFromDb).CurrentValues.SetValues(user);
        }

        public void DeleteUser(int id)
        {
            User user = GetUserById(id);
            var contactsToDelete = new List<Contact>();
            if (user.PrimaryContact != null)
                contactsToDelete.Add(context.Contacts.FirstOrDefault(_ => _.ID == user.PrimaryContact.ID));
            if (user.SecondaryContact != null)
                contactsToDelete.Add(context.Contacts.FirstOrDefault(_ => _.ID == user.SecondaryContact.ID));
            if (user.AdministrativeContact != null)
                contactsToDelete.Add(context.Contacts.FirstOrDefault(_ => _.ID == user.AdministrativeContact.ID));
            context.Contacts.RemoveRange(contactsToDelete);
            context.Users.Remove(user);
        }

        public void InsertUserContact(int userId, Contact contact)
        {
            User user = context.Users.Include("PrimaryContact").Single(_ => _.ID == userId);
            switch (contact.ContactType)
            {
                case ContactTypes.Primary:
                {
                    user.PrimaryContact = new PrimaryContact(contact);
                    break;
                }
                case ContactTypes.Administrative:
                {
                    user.AdministrativeContact = new AdministrativeContact(contact);
                    break;
                }
                case ContactTypes.Secondary:
                {
                    user.SecondaryContact = new SecondaryContact(contact);
                    break;
                }
            }
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
        }

        public IQueryable<User> SearchForUser(Expression<Func<User, bool>> predicate)
        {
            return context.Users
                .Include("PrimaryContact")
                .Include("SecondaryContact")
                .Include("AdministrativeContact")
                .Where(predicate);
        }

        public void DeleteUserContact(int userId, int contactId, int contactType)
        {
            User user = GetUserById(userId);
            Contact contact = context.Contacts.Find(contactId);

            switch (contactType)
            {
                case (int) ContactTypes.Primary:
                {
                    user.PrimaryContact = null;
                    break;
                }
                case (int) ContactTypes.Administrative:
                {
                    user.AdministrativeContact = null;
                    break;
                }
                case (int) ContactTypes.Secondary:
                {
                    user.SecondaryContact = null;
                    break;
                }
            }

            context.Entry(user).State = EntityState.Modified;
            context.Contacts.Remove(contact);
        }

        public void UpdateUserContact(Contact contact)
        {
            context.Contacts.Attach(contact);
            context.Entry(contact).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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