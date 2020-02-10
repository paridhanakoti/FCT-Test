using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserProductPurchaseApp.Data;
using UserProductPurchaseApp.Models;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;

namespace UserProductPurchaseApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ApplicationDbContext context;
        private List<ProductsModel> _Products;
        public ProductsController(ApplicationDbContext ctx)
        {
            context = ctx;
            if (!this.context.Products.Any())
            {
                _Products = new List<ProductsModel>()
                {
                    new ProductsModel() {Id = 1, Name = "Home Insurance", Description = "Full protection Home Insurance", Price = new decimal(100.00)},
                    new ProductsModel() {Id = 2, Name = "Flood Protection Insurance", Description = "Flood protection",Price = new decimal(30.70)},
                };

                context.Products.AddRange(_Products);
                context.SaveChanges();
            }
        }

        // GET api/Products?$OrderBy=Price        
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ProductsModel>>> Get()
        {
            //To verify that users can be obtained from context           
            return  await context.Products.ToListAsync();
        }

        // GET api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsModel>> Get(int id)
        {
            var prod = await context.Products.FindAsync(id);
            if (prod == null)
            {
                return NotFound();
            }
            
            return prod;          
        }

        // POST api/Products
        [HttpPost]
        public async Task<ActionResult<ProductsModel>> Post([FromBody] ProductsModel product)
        {
            var prodId = context.Products.Max(prod => prod.Id) + 1;
            product.Id = prodId;
            context.Products.Add(product);
            context.SaveChanges();

            return Ok();
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromODataUri] int Id, [FromBody] ProductsModel productChanges)
        {
            var product = context.Products.FindAsync(Id);
            if(product == null)
            {
                return BadRequest();
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

            //if (product != null)
            //{
            //    product.Name = productChanges.Name;
            //    product.Price = productChanges.Price;
            //    product.Description = productChanges.Description;

            //    context.Products.Update(product);
            //    context.SaveChanges();
            //}            
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductsModel>> Delete(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();

                return product;
            }
            else
            {
                return NotFound();
            }
        }

        private bool ProductsModelExists(int id)
        {
            return context.Products.Any(e => e.Id == id);
        }
    }
}
