using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name ="Urun Id")]//display ile productid yerine bu görünecek
        public int ProductId { get; set; }

        [Display(Name ="Urun Adı")]
        public string? Name { get; set; }

         [Display(Name ="Fiyat")]
        public decimal Price { get; set; }

         [Display(Name ="Resim")]
        public string Image { get; set; } = string.Empty;//? yerine bu şekilde de yapabilirim
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}
//1 iphone 14 1
//2 iphone 15 1