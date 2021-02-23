using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TiendaWebBicicletas.DAL;

namespace TiendaWebBicicletas.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        DbSet<T> _dbSet;

        private dbTiendaOnlineBicicletasEntities _DBEntity;

        public GenericRepository(dbTiendaOnlineBicicletasEntities DBEntity)
        {
            _DBEntity = DBEntity;
            _dbSet = _DBEntity.Set<T>();
        }

        public void Actualizar(T entity)
        {
            _dbSet.Attach(entity);
            _DBEntity.Entry(entity).State = EntityState.Modified;
            _DBEntity.SaveChanges();
        }

        public void ActualizarPorWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }

        public void Agregar(T entity)
        {
            _dbSet.Add(entity);
            _DBEntity.SaveChanges();
        }

        public void Eliminar(T entity)
        {
            if (_DBEntity.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void EliminarRangoWhereClause(Expression<Func<T, bool>> wherePredict)
        {
            List<T> entity = _dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {
                this.Eliminar(ent);
            }
        }

        public void ElminarWhereClause(Expression<Func<T, bool>> wherePredict)
        {
            T entity = _dbSet.Where(wherePredict).FirstOrDefault();
            this.Eliminar(entity);
        }

        public T GetId(int recordId)
        {
            return _dbSet.Find(recordId);
        }

        public IEnumerable<T> GetListaParametro(Expression<Func<T, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).ToList();
        }

        public T GetPorParametro(Expression<Func<T, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<T> GetRegistros()
        {
            return _dbSet.ToList();
        }

        public IQueryable<T> GetRegistrosIQueryable()
        {
            return _dbSet;
        }

        public IEnumerable<T> GetResuladoSqlProcedure(string query, params object[] parameters)
        {
            if (parameters != null)
            {
                return _DBEntity.Database.SqlQuery<T>(query, parameters).ToList();
            }
            else
                return _DBEntity.Database.SqlQuery<T>(query).ToList();

        }

        public int GetTamañoRegistros()
        {
            return _dbSet.Count();
        }

        public void InactivaYEliminaWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }
    }
}