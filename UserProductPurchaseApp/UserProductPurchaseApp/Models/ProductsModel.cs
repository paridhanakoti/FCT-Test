using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserProductPurchaseApp.Models
{
    public class ProductsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string Name { get; set; }        
        public string Description { get; set; }        
        public decimal Price { get; set; }
    }
}
