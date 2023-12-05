using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsApp.Models
{
    public class Repository //benim veritabanım
    {
        private static readonly List<Product> _products = new();
        private static readonly List<Category> _categories = new();
        static Repository(){
            _categories.Add(new Category {CategoryId = 1, Name = "Telefon"});
            _categories.Add(new Category {CategoryId = 2, Name = "Bilgisayar"});

            _products.Add(new Product {ProductId = 1, Name = "Iphone 14", Price = 40000, IsActive = true, Image = "1.jpg", CategoryId = 1});
            _products.Add(new Product {ProductId = 2, Name = "Iphone 15", Price = 50000, IsActive = false, Image = "2.jpg", CategoryId = 1});
            _products.Add(new Product {ProductId = 3, Name = "Iphone 16", Price = 60000, IsActive = true, Image = "3.jpg", CategoryId = 1});
            _products.Add(new Product {ProductId = 4, Name = "Iphone 17", Price = 70000, IsActive = false, Image = "4.jpg", CategoryId = 1});

            _products.Add(new Product {ProductId = 5, Name = "Macbook Air", Price = 80000, IsActive = false, Image = "5.jpg", CategoryId = 2});
            _products.Add(new Product {ProductId = 6, Name = "Macbook Pro", Price = 90000, IsActive = true, Image = "6.jpg", CategoryId = 2});

        }
        public static List<Product> Products{
            get{
                return _products;
            }
        }

        //Create.cshtlm de oluşturduğum yeni kayıt eklemek için ekledim
        public static void CreateProduct(Product entity){
            _products.Add(entity);
        }

        //Editle kayıtları güncellemek için ekledim
        public static void EditProduct(Product updateProduct){
            var entity = _products.FirstOrDefault(p => p.ProductId == updateProduct.ProductId);

            if(entity != null){
                //bunu böyle yapmamın nedeni EditIsActive diye yeni method yerine hepsini bu şekilde yaparak yeni bir değer girişi var o zaman güncelle derim eğer yeni değer girilmemişse de önceki değeri gir derim
                if(!string.IsNullOrEmpty(updateProduct.Name)){
                    entity.Name = updateProduct.Name;
                }
                entity.Price = updateProduct.Price;
                entity.Image = updateProduct.Image;
                entity.CategoryId = updateProduct.CategoryId;
                entity.IsActive = updateProduct.IsActive;
            }
        }

        //checkbox ile toplu güncelleme için
        //burda yeni method tanımlamamın nedeni toplu güncellemede checkboxa göre toplu güncelliyor ama üsteki methodu çağırarak yaparsam diğer alanları güncellemediğim için o alanları boş getiriyor bunu engellemek için yeni method tanımlayıp sadece bunu güncelle yaptım ya da onun yerine üsteki methodu if dögüsü ile kontrol ederek yapmam lazım
        public static void EditIsActive(Product updateProduct){
            var entity = _products.FirstOrDefault(p => p.ProductId == updateProduct.ProductId);

            if(entity != null){
                entity.IsActive = updateProduct.IsActive;
            }
        }

        //Delete silme işlemi için ekledim
        public static void DeleteProduct(Product deletedProduct){
            var entity = _products.FirstOrDefault(p => p.ProductId == deletedProduct.ProductId);

            if(entity != null){
                _products.Remove(entity);
            }
        }
        public static List<Category> Categories{
            get{
                return _categories;
            }
        }
    }
}