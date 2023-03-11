using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    { 
        
        private BloggieDbContext _bloggieDbContext;
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }

        // Dependency Enjection ==> Daha öncesinde DbContex'e özgü bir class tanımladık ve program.cs içerisinde bu classı service özelliklerinden uygulamamıza tanıttık. Bunu yapmaktaki amacımız ihtiyaç duyulan herhangi bir nesne içerisinde ihtiyaç duyduğumuz bu nesneyi çağırabilmek. Ve nesnenin içerisinde yeniden bu dbContex'i oluşturmadan, mevcutta onal dBcontext üzerine erişim sağlamaktı. Yani Dbconteximizi istediğimiz her classa enjekte edebiliyoruz.bu işleme dependency indejtion denmektedir. Bu örnekte bunu kullanabilmek için mevcut classımızda bir constructor oluşturduk ve parametre olarak dbContext nesnemizden bir argüman yolladık. Bu argüman classın tamamında kullanılamayacağı için bir private field oluştuduk ve bu field ile contsructurdan gelen argümanı eşitleyerek classımız içerisinde fieldımız üzerinden dbContex nesenesini kullanabiliyor hale geldik.



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            //var name= Request.Form["name"];
            //var displayName = Request.Form["displayName"];

            //var name = addTagRequest.Name;
            //var display = addTagRequest.DisplayName;

            var tag = new Tag
            {

                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,

            };

            _bloggieDbContext.Tags.Add(tag);
            _bloggieDbContext.SaveChanges();

            return View("Add");
        }

    }
}
