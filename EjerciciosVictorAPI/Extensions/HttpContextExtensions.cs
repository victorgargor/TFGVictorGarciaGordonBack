using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Extensions
{
    /// <summary>
    /// Extensiones para el objeto HttpContext.
    /// Estas extensiones proporcionan funcionalidades adicionales,
    /// como la inserción de parámetros de paginación en las respuestas HTTP.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Inserta los parámetros de paginación en las cabeceras de la respuesta HTTP.
        /// Calcula el número total de páginas y agrega esa información a las cabeceras.
        /// </summary>
        /// <typeparam name="T">El tipo de los elementos dentro de la consulta IQueryable.</typeparam>
        /// <param name="context">El objeto HttpContext actual.</param>
        /// <param name="queryable">La consulta IQueryable de los elementos a paginar.</param>
        /// <param name="cantidadRegistrosPorPagina">El número de registros por página para la paginación.</param>
        /// <returns>Una tarea asincrónica que representa la operación.</returns>
        public static async Task InsertarParametrosPaginacionEnRespuesta<T>(
            this HttpContext context, IQueryable<T> queryable, int cantidadRegistrosPorPagina)
        {
            // Obtener el conteo total de los elementos
            var conteo = await queryable.CountAsync();

            // Calcular el número total de páginas
            var totalPaginas = Math.Ceiling(conteo / (double)cantidadRegistrosPorPagina);

            // Agregar la cabecera 'totalPaginas' a la respuesta
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
            context.Response.Headers.Add("Access-Control-Expose-Headers", "totalPaginas");
        }
    }
}
