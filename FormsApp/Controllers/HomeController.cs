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

    [HttpGet]
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

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View();
    }

    [HttpPost]
    //sadece seçili ürünler gelsin istersem
    //public IActionResult Create([Bind("Name","Price")]Product model)
    public async Task<IActionResult> Create(Product model, IFormFile imageFile)//IFormFile imageFile içerisine eklenen img doğru olup olmadığını kontrol eder
    {
        var allowedExtensions = new[] {".jpg",".jpeg",".png"};//kabul edilecek uzantıları ekledim
        var extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır
        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk

        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);//Yüklenen resmi path.combine methodu içinde Directory.GetCurrentDirectory() ile şuan bulunan bu alnadaki dizininin içinde wwwroot/img içine imageFile.FileName ordaki remin adı ile kayıt et diyorum, ancak upload ettiğim klasörede aynı isimde başka bir klasör varsa ; random olarak isim verilebilir direk ya da if döngüsü ile isim kontrol edilir varsa sonuna başka bir şey eklenebilir. Burda da ben rastgele isim oluşturup direk ekledim onun içinde imageFile.FileName yerine randomFileName olarak kaydettim

        //yüklenen remin istediğim uzantıda oluğ olamadığını kontrol ediyorum
        if(imageFile != null){//kontrol etmeden resim dosyası seçilip seçilmediğini kontrol ediyorum
            if(!allowedExtensions.Contains(extension)){//kontrol işlemi yapıyorum istediğim uzatıda değilse hata mesajı verdim
                ModelState.AddModelError("","Geçerli bir resim seçiniz");
            }
        }

        if(ModelState.IsValid){//forma giren değerler validation hatası içermiyorsa çalışsın diyorum

            if(imageFile != null){//resim dosyası boş değilse 

                //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                using(var stream = new FileStream(path, FileMode.Create)){
                    await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                }
            }

            
            model.Image = randomFileName;//yeni eklediğimiz resim gözükebilsin diye eklememiz lazım

            model.ProductId = Repository.Products.Count + 1;

            Repository.CreateProduct(model);
            // return View();
            return RedirectToAction("Index");//yeni ürün kayıtından sonra index sayfasına git dedim
        }
        
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");

        return View(model);
        
    }

    public IActionResult Edit(int? id){
        if(id == null){
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);//o id ile eşleşen kayıt var mı diye baktım
        if(entity == null){
            return NotFound();
        }
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View(entity);//kayıtı sayfaya taşıdım
    }
    
}
