﻿namespace BDAProject.Web.Controllers
{
    using System.Threading.Tasks;

    using BDAProject.Data.Models;
    using BDAProject.Services.Data;
    using BDAProject.Web.ViewModels.Blog;
    using BDAProject.Web.ViewModels.Forum;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Microsoft.AspNetCore.Mvc;

    public class ForumController : Controller
    {
        private readonly IForumService forumService;
        private readonly UserManager<ApplicationUser> userManager;

        public ForumController(IForumService forumService, UserManager<ApplicationUser> userManager)
        {
            this.forumService = forumService;
            this.userManager = userManager;
        }

        public IActionResult All()
        {
            return this.View(this.forumService.GetAll());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> All(string postId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var username = this.userManager.GetUserName(this.User);

            await this.forumService.CreateLike(userId, postId, username);

            return this.Redirect($"/Forum/All");
        }

        [Authorize]
        public IActionResult AddPost()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(PostAddInputModel postInput)
        {
            var userId = this.userManager.GetUserId(this.User);
            var username = this.userManager.GetUserName(this.User);

            await this.forumService.CreatePost(userId, postInput.Text, username);

            return this.Redirect($"/Forum/All");
        }

        [Authorize]
        public IActionResult AddComment(string postId)
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentAddInputModel commentInput, string postId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var username = this.userManager.GetUserName(this.User);

            await this.forumService.CreateComment(userId, postId, username, commentInput.CommentText);

            return this.Redirect($"/Forum/All");
        }

        [Authorize]
        public IActionResult EditPost(string postId)
        {
            return this.View(this.forumService.GetPost(postId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost(string postId, string text)
        {
            await this.forumService.EditPost(postId, text);

            return this.Redirect($"/Forum/All");
        }

        [Authorize]
        public IActionResult EditComment(string commentId)
        {
            return this.View(this.forumService.GetComment(commentId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditComment(string commentId, string commentText)
        {
            await this.forumService.EditComment(commentId, commentText);

            return this.Redirect($"/Forum/All");
        }
    }
}
