using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/texto")]
    public class TextoController : ControllerBase
    {
        public Texto Texto { get; set; }

        public TextoController()
        {
            Texto = new Texto();
        }

        [HttpPost("contar")]
        public ActionResult Contar([FromBody] string texto)
        {
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Contar los caracteres del texto
                int contador = Texto.ContarCaracteres(texto);

                // Responder con un objeto estructurado
                return Ok(new
                {
                    caracteres = contador
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de que ocurra un error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al contar los caracteres",
                    error = ex.Message
                });
            }
        }

        [HttpPost("mayusculas")]
        public ActionResult PasarMayusculas([FromBody] string texto)
        {
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Convertir el texto a mayúsculas
                var resultado = Texto.ConvertirAMayusculas(texto);

                // Responder con el texto transformado
                return Ok(new
                {
                    textoConvertido = resultado
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de que ocurra un error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al convertir el texto a mayúsculas",
                    error = ex.Message
                });
            }
        }

        [HttpPost("minusculas")]
        public ActionResult PasarMinusculas([FromBody] string texto)
        {
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Convertir el texto a minúsculas
                var resultado = Texto.ConvertirAMinusculas(texto);

                // Responder con el texto transformado
                return Ok(new
                {
                    textoConvertido = resultado
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de que ocurra un error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al convertir el texto a minúsculas",
                    error = ex.Message
                });
            }
        }

        [HttpPost("repetidas")]
        public ActionResult MostrarRepetidas([FromBody] string texto)
        {
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Contar las palabras repetidas en el texto
                var resultado = Texto.ContarRepetidas(texto);

                // Responder con el resultado
                return Ok(new
                {
                    palabrasRepetidas = resultado
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de que ocurra un error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al contar las palabras repetidas",
                    error = ex.Message
                });
            }
        }

        [HttpPost("reemplazar")]
        public ActionResult Reemplazar([FromBody] string texto)
        {
            texto = string.IsNullOrEmpty(texto) ? this.ObtenerTextoPredeterminado() : texto;

            try
            {
                // Reemplazar las palabras en el texto
                var resultado = Texto.ReemplazarPalabra(texto);

                // Responder con el texto reemplazado
                return Ok(new
                {
                    textoReemplazado = resultado
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de que ocurra un error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al reemplazar las palabras",
                    error = ex.Message
                });
            }
        }

        [HttpPost("concatenar")]
        public ActionResult MostrarTiempoConcatenar([FromBody] ConcatenacionRequest request)
        {
            // Verificar si el número de veces es mayor que 1000
            if (request.Veces > 100000)
            {
                return BadRequest(new
                {
                    mensaje = "El número de veces para concatenar no puede ser mayor a 100000."
                });
            }

            // Si el texto está vacío o nulo, asignamos el texto predeterminado
            string texto = string.IsNullOrEmpty(request.Texto) ? this.ObtenerTextoPredeterminado() : request.Texto;
            int veces = request.Veces;

            try
            {
                // Concatenar el texto varias veces y medir el tiempo
                (int longitud, long tiempo) = Texto.ConcatenarTexto(texto, veces);

                // Responder con el tiempo y la longitud, agregando la unidad de tiempo
                return Ok(new
                {
                    mensaje = "Concatenación completada con éxito",
                    tiempoConcatenacion = $"{tiempo} ms", // Aquí se agrega la unidad de milisegundos
                    longitudTexto = longitud
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de que ocurra un error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al concatenar el texto",
                    error = ex.Message
                });
            }
        }

        private string ObtenerTextoPredeterminado()
        {
            return "Proconsi es una empresa de Tecnologías de la Información y la Comunicación especializada en el desarrollo e integración de soluciones informáticas para todo tipo de empresas. Más de tres décadas de experiencia avalan a una compañía tan flexible como responsable. Cuenta con un equipo multidisciplinar de más de 120 profesionales cualificados, expertos y comprometidos con un único objetivo: hallar la solución tecnológica exacta para cada cliente. Proconsi es especialista en la creación y el desarrollo de software de gestión, consultoría tecnológica, dirección y gestión de proyectos I+D+i basados en TIC, soporte técnico, aplicaciones móviles y fomento de tendencias en nuevas tecnologías, como el cloud computing.";
        }
    }
}
