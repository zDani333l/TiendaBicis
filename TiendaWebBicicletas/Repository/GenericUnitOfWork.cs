using System;
using System.Collections.Generic;
using System.Web;
using TiendaWebBicicletas.DAL;

namespace TiendaWebBicicletas.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private dbTiendaOnlineBicicletasEntities context = new dbTiendaOnlineBicicletasEntities();

        public IRepository<T> GetRepositoryInstance<T>() where T : class
        {
            return new GenericRepository<T>(context);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
    }
}