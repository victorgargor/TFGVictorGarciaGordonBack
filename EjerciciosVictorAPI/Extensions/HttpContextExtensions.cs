using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task InsertarParametrosPaginacionEnRespuesta<T>(
       this HttpContext context, IQueryable<T> queryable, int cantidadRegistrosPorPagina)
        {
            var conteo = await queryable.CountAsync();
            var totalPaginas = Math.Ceiling(conteo / (double)cantidadRegistrosPorPagina);

            // Agregar la cabecera 'totalPaginas' a la respuesta
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}
