using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }


        // Tagleri listeleme için
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Seçili Olan Tag İçin
        public string SelectedTag { get; set; }
    }
}
