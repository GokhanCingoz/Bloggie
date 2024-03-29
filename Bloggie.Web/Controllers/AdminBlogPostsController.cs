﻿using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

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
        //Edit Get Action
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogpost = await blogPostRepository.GetAsync(id);
            var tagsFromDomainModel = await tagRepository.GetAllAsync();

            if (blogpost != null)
            {
                //domain modeldaki verilerimizi viewmodela mapliyoruz.
                var model = new EditBlogPostRequest
                {
                    Id = blogpost.Id,
                    PublishedDate = blogpost.PublishedDate,
                    Visible = blogpost.Visible,
                    PageTitle = blogpost.PageTitle,
                    Author = blogpost.Author,
                    Content = blogpost.Content,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    UrlHandle = blogpost.UrlHandle,
                    Heading = blogpost.Heading,
                    ShortDescription = blogpost.ShortDescription,
                    Tags = tagsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogpost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //view modeldan gelenler ile domain modeli maplayelim.
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,
                PageTitle = editBlogPostRequest.PageTitle,
                Author = editBlogPostRequest.Author,
                Content = editBlogPostRequest.Content,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Heading = editBlogPostRequest.Heading,
                ShortDescription = editBlogPostRequest.ShortDescription,
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectedTags;

            //Submit ediyoruz
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
    }
}
