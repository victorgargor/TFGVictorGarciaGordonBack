using EjerciciosVictorAPI.Models; 
using Microsoft.AspNetCore.Mvc; 

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para manejar operaciones con texto.
    /// </summary>
    [ApiController] 
    [Route("api/texto")] 
    public class TextoController : ControllerBase
    {
        /// <summary>
        ///  Propiedad que instancia el objeto Texto para realizar las operaciones
        /// </summary>
        public Texto Texto { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia de la clase Texto.
        /// </summary>
        public TextoController()
        {
            Texto = new Texto(); 
        }

        /// <summary>
        /// Cuenta el número de carácteres en el texto.
        /// </summary>
        /// <param name="texto">Texto para contar los carácteres.</param>
        /// <returns>Resultado con el número de caracteres.</returns>
        [HttpPost("contar")] 
        public ActionResult Contar([FromBody] string texto)
        {
            // Si no se proporciona texto, se utiliza el texto predeterminado
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Se cuentan los carácteres del texto
                int contador = Texto.ContarCaracteres(texto);

                // Devolución del resultado
                return Ok(new
                {
                    caracteres = contador
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al contar los caracteres",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Convierte el texto a mayúsculas.
        /// </summary>
        /// <param name="texto">Texto para convertir a mayúsculas.</param>
        /// <returns>Texto convertido a mayúsculas.</returns>
        [HttpPost("mayusculas")] 
        public ActionResult PasarMayusculas([FromBody] string texto)
        {
            // Si no se proporciona texto, se utiliza el texto predeterminado
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Se convierte el texto a mayúsculas
                var resultado = Texto.ConvertirAMayusculas(texto);

                // Devolución del resultado
                return Ok(new
                {
                    textoConvertido = resultado
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al convertir el texto a mayúsculas",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Convierte el texto a minúsculas.
        /// </summary>
        /// <param name="texto">Texto para convertir a minúsculas.</param>
        /// <returns>Texto convertido a minúsculas.</returns>
        [HttpPost("minusculas")] 
        public ActionResult PasarMinusculas([FromBody] string texto)
        {
            // Si no se proporciona texto, se utiliza el texto predeterminado
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Se convierte el texto a minúsculas
                var resultado = Texto.ConvertirAMinusculas(texto);

                // Devolución del resultado
                return Ok(new
                {
                    textoConvertido = resultado
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al convertir el texto a minúsculas",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Muestra las palabras que se repiten en el texto.
        /// </summary>
        /// <param name="texto">Texto para analizar las palabras repetidas.</param>
        /// <returns>Palabras repetidas encontradas en el texto.</returns>
        [HttpPost("repetidas")]
        public ActionResult MostrarRepetidas([FromBody] string texto)
        {
            // Si no se proporciona texto, se utiliza el texto predeterminado
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Se cuentan las palabras repetidas 
                var resultado = Texto.ContarRepetidas(texto);

                // Devolución del resultado
                return Ok(new
                {
                    palabrasRepetidas = resultado
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al contar las palabras repetidas",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Reemplaza las palabras en el texto.
        /// </summary>
        /// <param name="texto">Texto para reemplazar palabras.</param>
        /// <returns>Texto con las palabras reemplazadas.</returns>
        [HttpPost("reemplazar")] 
        public ActionResult Reemplazar([FromBody] string texto)
        {
            // Si no se proporciona texto, se utiliza el texto predeterminado
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Se reemplazan las palabras en el texto
                var resultado = Texto.ReemplazarPalabra(texto);

                // Devolución del resultado
                return Ok(new
                {
                    textoReemplazado = resultado
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al reemplazar las palabras",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Concatenar el texto varias veces y medir el tiempo en ms.
        /// </summary>
        /// <param name="request">Objeto que contiene el texto y el número de veces a concatenar.</param>
        /// <returns>Tiempo de concatenación (ms) y longitud del texto resultante.</returns>
        [HttpPost("concatenar")] // Ruta para concatenar el texto
        public ActionResult MostrarTiempoConcatenar([FromBody] ConcatenacionRequest request)
        {
            // Valido que el número de veces sea mayor a 0 y menor o igual a 100000
            if (request.Veces > 100000 || request.Veces <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "El número de veces para concatenar no puede ser mayor a 100000 ni menor o igual a 0."
                });
            }

            // Si el texto está vacío, se utiliza el texto predeterminado
            string texto = string.IsNullOrEmpty(request.Texto) ? this.ObtenerTextoPredeterminado() : request.Texto;
            int veces = request.Veces;

            try
            {
                // Se concatena el texto y se mide el tiempo
                (int longitud, long tiempo) = Texto.ConcatenarTexto(texto, veces);

                // Devolución del resultado
                return Ok(new
                {
                    mensaje = "Concatenación completada con éxito",
                    tiempoConcatenacion = $"{tiempo} ms", 
                    longitudTexto = longitud
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al concatenar el texto",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Devuelve el texto predeterminado en caso de que no se reciba texto por parte del usuario.
        /// </summary>
        /// <returns>Texto predeterminado.</returns>
        private string ObtenerTextoPredeterminado()
        {
            return "Proconsi es una empresa de Tecnologías de la Información y la Comunicación especializada en el desarrollo e integración de soluciones informáticas para todo tipo de empresas. Más de tres décadas de experiencia avalan a una compañía tan flexible como responsable. Cuenta con un equipo multidisciplinar de más de 120 profesionales cualificados, expertos y comprometidos con un único objetivo: hallar la solución tecnológica exacta para cada cliente. Proconsi es especialista en la creación y el desarrollo de software de gestión, consultoría tecnológica, dirección y gestión de proyectos I+D+i basados en TIC, soporte técnico, aplicaciones móviles y fomento de tendencias en nuevas tecnologías, como el cloud computing.";
        }
    }
}

