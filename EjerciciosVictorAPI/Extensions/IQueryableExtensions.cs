using EjerciciosVictorAPI.DTOs;

namespace EjerciciosVictorAPI.Extensions
{
    /// <summary>
    /// Extensiones para el tipo IQueryable.
    /// Estas extensiones permiten agregar funcionalidades adicionales a las consultas de bases de datos.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Aplica paginación a una consulta IQueryable.
        /// Filtra los elementos de la consulta según la página y la cantidad de registros por página.
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos dentro de la consulta IQueryable.</typeparam>
        /// <param name="queryable">La consulta IQueryable a paginar.</param>
        /// <param name="paginacion">Un objeto que contiene los parámetros de paginación (número de página y cantidad de registros).</param>
        /// <returns>Una consulta IQueryable que ha sido paginada.</returns>
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacion)
        {
            // Aplicar el salto de registros según el número de página y la cantidad de registros por página
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistros)
                .Take(paginacion.CantidadRegistros);
        }
    }
}
