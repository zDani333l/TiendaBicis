using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TiendaWebBicicletas.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetRegistros();
        IQueryable<T> GetRegistrosIQueryable();
        int GetTamañoRegistros();
        void Agregar(T entity);
        void Actualizar(T entity);
        void ActualizarPorWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict);
        T GetId(int recordId);
        void Eliminar(T entity);
        void ElminarWhereClause(Expression<Func<T, bool>> wherePredict);
        void EliminarRangoWhereClause(Expression<Func<T, bool>> wherePredict);
        void InactivaYEliminaWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict);
        T GetPorParametro(Expression<Func<T, bool>> wherePredict);
        IEnumerable<T> GetListaParametro(Expression<Func<T, bool>> wherePredict);
        IEnumerable<T> GetResuladoSqlProcedure(string query, params object[] parameters);
    }
}