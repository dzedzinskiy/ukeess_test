using System;
using Models.DataContexts;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppContext _context;

        public UnitOfWork(IAppContext context)
        {
            _context = context;
        }

        public void Dispose()
        {

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
