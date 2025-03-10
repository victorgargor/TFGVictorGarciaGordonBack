using System.Threading;
using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/itemseparator")]
    public class ItemSeparatorController : ControllerBase
    {
        public ItemSeparator ItemSeparator { get; set; }

        public ItemSeparatorController()
        {
            string valorPorDefecto = "Bread$$##12.5$$##10";
            ItemSeparator = new ItemSeparator(valorPorDefecto);
        }

        [HttpPost]
        public ActionResult SepararCadena([FromBody] ItemSeparatorRequest request)
        {
            try
            {
                // Usar el valor de request.Stdln si existe, si no, utilizar el valor por defecto de la propiedad ItemSeparator
                string stdln = request.Stdln ?? ItemSeparator.GetName() + "$$##" + ItemSeparator.GetPrice() + "$$##" + ItemSeparator.GetQuantity();

                // Crear una nueva instancia de ItemSeparator con el valor de stdln
                var itemSeparator = new ItemSeparator(stdln);

                // Devolver la respuesta con la información separada
                return Ok(
                    $"Item Name: {itemSeparator.GetName()}\n" +
                    $"Item Price: {itemSeparator.GetPrice()}\n" +
                    $"Item Quantity: {itemSeparator.GetQuantity()}"
                );
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al separar alguno de los datos",
                    error = ex.Message
                });
            }
        }
    }
}
