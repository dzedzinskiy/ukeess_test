using System;
using Models.DataContexts;

namespace BLL.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        void SaveChanges();
    }

    public class UnitOfWork: IUnitOfWork
    {
        private readonly DAL.UnitOfWork.UnitOfWork uof;

        public UnitOfWork(IAppContext context)
        {
            uof = new DAL.UnitOfWork.UnitOfWork(context);
        }

        public void Dispose()
        {
            uof.Dispose();
        }

        public void SaveChanges()
        {
            uof.SaveChanges();
        }
    }
}
