using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name ="Urun Id")]//display ile productid yerine bu görünecek
       // [BindNever] Buffer alan gelmesin istiyorsam böyle yapmalıyım
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]//max 100 karakter girebilir
        [Display(Name ="Urun Adı")]
        public string Name { get; set; } = null!;//nullabel string konusu önemli burdaki amaç ben boş bir değer döndermiyiceğimi söylüyorum bu konuda detaylı anlatım kursta 51. ders(yani artık string içine default nul bir değer atıyabiliyordum artık ise buna izin verirsem atabilirim)

        [Required]
        [Range(0,1000000)]//bu fiyat aralığında değer gir dedim
        [Display(Name ="Fiyat")]
        public decimal? Price { get; set; }

        // [Required]
        [Display(Name ="Resim")]
        public string Image { get; set; } = string.Empty;//? yerine bu şekilde de yapabilirim
        public bool IsActive { get; set; }

        [Required]
        [Display(Name ="Category")]
        public int? CategoryId { get; set; }
    }
}
//1 iphone 14 1
//2 iphone 15 1