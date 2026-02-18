using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsServices _productsServices;
        public ProductsController(IProductsServices productsServices) 
        {
            _productsServices = productsServices;
        }
        [HttpGet("all-products")]
        public async Task<IActionResult> Get()
        {
            var products = await _productsServices.GetInfoProducts();

            return Ok(products);
        }
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                var product = await _productsServices.GetInfoProduct(id);

                return Ok(product);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> Post([FromBody] Products products)
        {
            try
            {
                await _productsServices.AddProduct(products.Name,products.Category,products.Supplier,
                    products.Quantity,products.Price);

                return Ok("Добавлена новая запись");
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpPut("uptade-product/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Products products)
        {
            try
            {
                await _productsServices.UptadeProduct(id, products.Name, products.Category, products.Supplier
                    ,products.Quantity, products.Price);
                return Ok();
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productsServices.DeleteProduct(id);

                return Ok($"Удалена запись id: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpGet("check-stock/{id}/{quantity}")]
        public async Task<IActionResult> Check(int id, int quantity)
        {
            try
            {
                await _productsServices.CheckQuantity(id, quantity);

                return Ok($"привет");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Пытаетесь купить больше!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpGet("summ/{id}/{quantity}")]
        public async Task<IActionResult> SummGet(int id, int quantity)
        {
            try
            {
                var result = await _productsServices.SummProduct(id, quantity);

                return Ok(result);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Ошибка");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpPut("decrease/{id}/{quantity}")]
        public async Task<IActionResult> DecreaseGet(int id, int quantity)
        {
            try
            {
                var result = await _productsServices.DecreaseProduct(id, quantity);

                return Ok(result);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Ошибка");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}
