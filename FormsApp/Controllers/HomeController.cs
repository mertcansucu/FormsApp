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
        var extension = "";//alt tarafta da kullandığım için başta tanımladım
        //yüklenen remin istediğim uzantıda oluğ olamadığını kontrol ediyorum
        if(imageFile != null){//kontrol etmeden resim dosyası seçilip seçilmediğini kontrol ediyorum

            var allowedExtensions = new[] {".jpg",".jpeg",".png"};//kabul edilecek uzantıları ekledim
            extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır

            if(!allowedExtensions.Contains(extension)){//kontrol işlemi yapıyorum istediğim uzatıda değilse hata mesajı verdim
                ModelState.AddModelError("","Geçerli bir resim seçiniz");
            }
        }

        if(ModelState.IsValid){//forma giren değerler validation hatası içermiyorsa çalışsın diyorum

            if(imageFile != null){//resim dosyası boş değilse 
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk

                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);//Yüklenen resmi path.combine methodu içinde Directory.GetCurrentDirectory() ile şuan bulunan bu alnadaki dizininin içinde wwwroot/img içine imageFile.FileName ordaki remin adı ile kayıt et diyorum, ancak upload ettiğim klasörede aynı isimde başka bir klasör varsa ; random olarak isim verilebilir direk ya da if döngüsü ile isim kontrol edilir varsa sonuna başka bir şey eklenebilir. Burda da ben rastgele isim oluşturup direk ekledim onun içinde imageFile.FileName yerine randomFileName olarak kaydettim

                //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                using(var stream = new FileStream(path, FileMode.Create)){
                    await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                }
                //resim varsa oluşacak
                model.Image = randomFileName;//yeni eklediğimiz resim gözükebilsin diye eklememiz lazım

                model.ProductId = Repository.Products.Count + 1;

                Repository.CreateProduct(model);
                // return View();
                return RedirectToAction("Index");//yeni ürün kayıtından sonra index sayfasına git dedim
            }

        }
        
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");

        return View(model);
        
    }

    [HttpGet]
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

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile){//IFormFile? resim seçilme zorunluluğunu kaldırdım
        if(id != model.ProductId){
            return NotFound();
        }

        if(ModelState.IsValid){
            if(imageFile != null){//resim dosyası boş değilse 
                var extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk

                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);//Yüklenen resmi path.combine methodu içinde Directory.GetCurrentDirectory() ile şuan bulunan bu alnadaki dizininin içinde wwwroot/img içine imageFile.FileName ordaki remin adı ile kayıt et diyorum, ancak upload ettiğim klasörede aynı isimde başka bir klasör varsa ; random olarak isim verilebilir direk ya da if döngüsü ile isim kontrol edilir varsa sonuna başka bir şey eklenebilir. Burda da ben rastgele isim oluşturup direk ekledim onun içinde imageFile.FileName yerine randomFileName olarak kaydettim

                //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                using(var stream = new FileStream(path, FileMode.Create)){
                    await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                }
                model.Image = randomFileName;
            }
            Repository.EditProduct(model);
            return RedirectToAction("Index");
        }
        //else durumu düşün burayı eğer hata olursa sayfayı yeniden yükledim
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);
    }

    //silme işleminde kişi eğer ürünü silme istiyorsa delete butonuna basıp get ile bilgileri DeleteConfirm göderip orda tekrar silmek istiyor musun diye soruyorum oda evet derse post ile bu işlemi onaylamak veriyi siliyorum
    [HttpGet]//get ile deleted işlemi
    public IActionResult Delete(int? id)
    {
        if(id == null)
        {
            return NotFound();        
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
        if(entity == null)
        {
            return NotFound();
        }


        //burayı kapatma nedenim direk silme yapıyorum Get delete, onun yerine silmek istiyor musun gibi soru kullanmak
        // Repository.DeleteProduct(entity);
        // return RedirectToAction("Index");//silindikten sonra listeyi getir

        return View("DeleteConfirm", entity);
    }

    [HttpPost]//post deleted
    public IActionResult Delete(int id, int ProductId)
    {
        if(id != ProductId)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == ProductId);
        if(entity == null)
        {
            return NotFound();
        }

        Repository.DeleteProduct(entity);
        return RedirectToAction("Index");
    }

    [HttpPost]//toplu edit yani güncelleme için yaptım modernbinding işlemi
    public IActionResult EditProducts(List<Product> Products)
    {
        foreach (var product in Products)
        {
            Repository.EditIsActive(product);
        }
        return RedirectToAction("Index");
    }
    
}
