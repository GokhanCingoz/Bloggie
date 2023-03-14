using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagInterface tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagInterface tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                Author = addBlogPostRequest.Author,
                Content = addBlogPostRequest.Content,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                PageTitle = addBlogPostRequest.PageTitle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                ShortDescription = addBlogPostRequest.ShortDescription,
                UrlHandle = addBlogPostRequest.UrlHandle,
                Visible = addBlogPostRequest.Visible,

            };

            var selectedTags = new List<Tag>();

            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existinTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existinTag != null)
                {
                    selectedTags.Add(existinTag);
                }
            }

            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPost = await blogPostRepository.GetAllAsync();
            return View(blogPost);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsFromDomainModel = await tagRepository.GetAllAsync();

            if (blogPost!=null)
            {

                //domain modeldeki verilerimizi viewmodela mapliyoruz.
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Tags = tagsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }


            return View(null);
        }
    }
}
