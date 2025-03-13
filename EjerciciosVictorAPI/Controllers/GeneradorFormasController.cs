using Microsoft.AspNetCore.Mvc;
using EjerciciosVictorAPI.Models;
using System;
using System.Collections.Generic;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador encargado de generar formas geométricas (círculos, cuadrados y triángulos) según los parámetros especificados por el usuario.
    /// </summary>
    [ApiController]
    [Route("api/formas")]
    public class GeneradorFormasController : ControllerBase
    {
        /// <summary>
        /// Instancia de la clase Random utilizada para generar números aleatorios, para las dimensiones de las formas).
        /// </summary>
        private static Random aleatorio = new Random();

        /// <summary>
        /// Método para generar las formas geométricas basadas en la solicitud del usuario.
        /// </summary>
        /// <param name="request">Objeto que contiene la cantidad de cada tipo de forma a generar.</param>
        /// <returns>Una respuesta con las formas generadas o un error si la solicitud no es válida.</returns>
        [HttpPost]
        public IActionResult GenerarFormas([FromBody] FormasRequest request)
        {
            // Si no se introduce ninguna forma
            if (request.Circulos <= 0 && request.Triangulos <= 0 && request.Cuadrados <= 0)
            {        
                return BadRequest("Debe especificar al menos una forma para generar.");
            }


            // Validación de que el número de formas de cada tipo sea positivo
            if (request.Circulos < 0)
            {
                return BadRequest("El valor de los círculos debe ser un número entero positivo mayor que cero.");
            }

            if (request.Cuadrados < 0)
            {
                return BadRequest("El valor de los cuadrados debe ser un número entero positivo mayor que cero.");
            }

            if (request.Triangulos < 0)
            {
                return BadRequest("El valor de los triángulos debe ser un número entero positivo mayor que cero.");
            }


            // Lista para almacenar las distintas formas
            List<Forma> todasLasFormas = new List<Forma>();

            // Generar círculos
            for (int i = 0; i < request.Circulos; i++)
            {
                // Generar propiedades aleatorias
                double radio = aleatorio.Next(1, 20);
                var circulo = new Circulo(radio);
                circulo.Color = circulo.GenerarColorAleatorio();
                circulo.Centro = circulo.GenerarCoordenadasAleatorias();
                todasLasFormas.Add(circulo);
            }

            // Generar cuadrados
            for (int i = 0; i < request.Cuadrados; i++)
            {
                double lado = aleatorio.Next(1, 20);
                var cuadrado = new Cuadrado(lado);
                cuadrado.Color = cuadrado.GenerarColorAleatorio();
                cuadrado.Centro = cuadrado.GenerarCoordenadasAleatorias();
                todasLasFormas.Add(cuadrado);
            }

            // Generar triángulos
            for (int i = 0; i < request.Triangulos; i++)
            {
                double baseTriangulo = aleatorio.Next(1, 20);
                double altura = aleatorio.Next(1, 20);
                var triangulo = new Triangulo(baseTriangulo, altura);
                triangulo.Color = triangulo.GenerarColorAleatorio();
                triangulo.Centro = triangulo.GenerarCoordenadasAleatorias();
                todasLasFormas.Add(triangulo);
            }

            // Crear un objeto de respuesta con todas las formas
            var respuesta = new
            {
                // Todas las formas
                TodasLasFormas = todasLasFormas.Select(frm => new
                {
                    // Tipo de la forma (Círculo, Cuadrado, Triángulo)
                    Tipo = frm.GetType().Name, 
                    frm.Color,
                    // Serializo el cenro
                    Centro = new { X = frm.Centro.x, Y = frm.Centro.y },
                    Area = frm.CalcularArea(),
                    Propiedades = frm.ObtenerPropiedades()
                }),
                // Las agrupadas
                Agrupadas = new
                {
                    Circulos = todasLasFormas.OfType<Circulo>().Select(cir => new
                    {
                        cir.Radio,
                        cir.Color,
                        Centro = new { X = cir.Centro.x, Y = cir.Centro.y },
                        Area = cir.CalcularArea()
                    }),
                    Cuadrados = todasLasFormas.OfType<Cuadrado>().Select(cua => new
                    {
                        cua.Lado,
                        cua.Color,
                        Centro = new { X = cua.Centro.x, Y = cua.Centro.y },
                        Area = cua.CalcularArea()
                    }),
                    Triangulos = todasLasFormas.OfType<Triangulo>().Select(tri => new
                    {
                        tri.Base,
                        tri.Altura,
                        tri.Color,
                        Centro = new { X = tri.Centro.x, Y = tri.Centro.y },
                        Area = tri.CalcularArea()
                    })
                }
            };

            // Devuelvo las formas 
            return Ok(respuesta);
        }
    }
}

