using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProductPurchaseApp.Data;
using UserProductPurchaseApp.Models;

namespace UserProductPurchaseApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private List<PurchaseModel> _Purchases;
        public PurchasesController(ApplicationDbContext ctx, UserManager<ApplicationUser> userMgr)
        {
            context = ctx;
            userManager = userMgr;

            if (!this.context.Purchases.Any())
            {
                //get users to get the Ids
                var userIds = this.context.Users.Select(u => u.Id).ToList();

                _Purchases = new List<PurchaseModel>()
                {
                    new PurchaseModel() {Id = 1, UserId = userIds.First(), ProductId = 1},
                    new PurchaseModel() {Id = 2, UserId = userIds.Last(), ProductId = 2},
                };

                context.Purchases.AddRange(_Purchases);
                context.SaveChanges();
            }
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseModel>>> ViewAllPurchases()
        {
            //when a user is logged in, they are authorized to see only their purchases
            var userName = userManager.GetUserId(HttpContext.User);
            var userId = context.Users.SingleOrDefault(u => u.UserName.Equals(userName))?.Id;

            if(userId != null)
            {
                return await context.Purchases.Where(purchase => purchase.UserId.Equals(userId)).ToListAsync();
            }
            
            return null;
        }

        //// GET: api/Purchases/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PurchaseModel>> ViewPurchase(int id)
        //{
        //    var purchaseModel = await context.Purchases.FindAsync(id);

        //    if (purchaseModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return purchaseModel;
        //}

        //// PUT: api/Purchases/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> updatePurchase(int id, PurchaseModel purchaseModel)
        //{
        //    if (id != purchaseModel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(purchaseModel).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PurchaseModelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Purchases
        [HttpPost]
        public async Task<ActionResult<PurchaseModel>> Purchase(PurchaseModel purchaseModel)
        {
            //a logged in user can make purchases only for themselves.
            //check the purchase model userId is same as logged in user, otherwise restrict purchase
            var userName = userManager.GetUserId(HttpContext.User);
            var loggedInUserId = context.Users.SingleOrDefault(u => u.UserName.Equals(userName))?.Id;

            if (loggedInUserId != purchaseModel.UserId)
            {
                return Forbid();
            }
            //give an Id
            var purchaseId = context.Purchases.Max(p => p.Id) + 1;
            purchaseModel.Id = purchaseId;

            context.Purchases.Add(purchaseModel);
            await context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseModel>> CancelPurchase(int id)
        {            
            var userName = userManager.GetUserId(HttpContext.User);
            var loggedInUserId = context.Users.SingleOrDefault(u => u.UserName.Equals(userName))?.Id;

            var purchaseModel = await context.Purchases.FindAsync(id);
            if (purchaseModel == null)
            {
                return NotFound();
            }

            //a logged in user can cancel only their purchases.
            //check the purchase model userId is same as logged in user, otherwise restrict cancel
            if (loggedInUserId != purchaseModel.UserId)
            {
                return Forbid();
            }

            context.Purchases.Remove(purchaseModel);
            await context.SaveChangesAsync();

            return purchaseModel;
        }

        private bool PurchaseModelExists(int id)
        {
            return context.Purchases.Any(e => e.Id == id);
        }
    }
}
