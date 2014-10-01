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
    public class UserRepository : Repository<User, int>, IUserRepository<User, int>
    {
        private bool disposed;

        private IAppContext context { get; set; }

        public UserRepository(IAppContext context) : base((DbContext) context)
        {
            this.context = context;
        }

        public User GetUserWithContactsById(int userId)
        {
            return base.SearchFor(_ => _.ID == userId, "PrimaryContact,SecondaryContact,AdministrativeContact").Single();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return base.SearchFor(null, "PrimaryContact,SecondaryContact,AdministrativeContact").AsEnumerable();
        }


        public void DeleteUserWithContacts(int userId)
        {
            User user = GetUserWithContactsById(userId);
            var contactsToDelete = new List<Contact>();
            if (user.PrimaryContact != null)
                contactsToDelete.Add(context.Contacts.FirstOrDefault(_ => _.ID == user.PrimaryContact.ID));
            if (user.SecondaryContact != null)
                contactsToDelete.Add(context.Contacts.FirstOrDefault(_ => _.ID == user.SecondaryContact.ID));
            if (user.AdministrativeContact != null)
                contactsToDelete.Add(context.Contacts.FirstOrDefault(_ => _.ID == user.AdministrativeContact.ID));
            context.Contacts.RemoveRange(contactsToDelete);
            base.Delete(user);
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
            base.Update(user);
        }

        public void DeleteUserContact(int userId, int contactId, int contactType)
        {
            User user = GetUserWithContactsById(userId);
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