using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{

    public HomeController()
    {
         
    }

    public IActionResult Index(string searchString,string category)
    {
        //search 
        var products = Repository.Products; 

        if(!String.IsNullOrEmpty(searchString)){//arama yaptım
            ViewBag.searchString = searchString;//aranan kelime kalsın diye
            products = products.Where(p => p.Name.ToLowerInvariant().Contains(searchString)).ToList();
        }

        if(!String.IsNullOrEmpty(category) && category != "0"){
            //category != "0" filtrede hepsi seçersem tüm liste gelsin diye
            products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }

        /*1. yolkategori bilgileri sayfa üzerine taşıdım Filtreleme
        //kategori bilgileri sayfa üzerine taşıdım Filtreleme
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name",category);//category de ekledim çünkü ben telefonu seçip filtreleme yaptığımda o orda yazılı kalsın diye
        */
        

        //2.yol kategori bilgileri sayfa üzerine taşıdım Filtreleme
        
        var model = new ProductViewModel{
            Products = products,
            Categories = Repository.Categories,
            SelectedCategory = category
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
}
