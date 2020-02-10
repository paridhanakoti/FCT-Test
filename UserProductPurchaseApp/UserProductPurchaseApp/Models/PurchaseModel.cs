using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserProductPurchaseApp.Models
{
    public class PurchaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserId is required")]        
        public string UserId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public int ProductId { get; set; }        
    }
}
