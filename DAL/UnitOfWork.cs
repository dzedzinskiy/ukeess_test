using System;
using DAL.DataContexts;
using DAL.Models.Contacts;
using DAL.Repositories;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IAppContext context;
        private GenericRepository<Contact> contactRepository;
        private bool disposed;
        private UserRepository userRepository;

        public UnitOfWork(IAppContext context)
        {
            this.context = context;
        }

        public UserRepository UserRepository
        {
            get { return userRepository ?? (userRepository = new UserRepository(context)); }
        }

        public GenericRepository<Contact> ContactRepository
        {
            get { return contactRepository ?? (contactRepository = new GenericRepository<Contact>(context)); }
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