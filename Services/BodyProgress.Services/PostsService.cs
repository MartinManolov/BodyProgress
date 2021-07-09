using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using BodyProgress.Services.Common;
using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels;
using BodyProgress.Web.ViewModels.ViewInputModels;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace BodyProgress.Services
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> _postsRepository;
        private readonly IUploadMediaService _uploadMediaService;
        private readonly Cloudinary _cloudinary;

        public PostsService(IDeletableEntityRepository<Post> postsRepository,
            IUploadMediaService uploadMediaService,
            Cloudinary cloudinary)
        {
            this._postsRepository = postsRepository;
            this._uploadMediaService = uploadMediaService;
            this._cloudinary = cloudinary;
        }

        public async Task Create(PostInputModel input, string userId)
        {
            var imageName = userId + Guid.NewGuid().ToString();
            var imageUrl = await this._uploadMediaService.UploadImage(_cloudinary, input.Image, imageName);
            var post = new Post()
            {
                Date = DateTime.UtcNow,
                OwnerId = userId,
                TextContent = input.TextContent,
                IsPublic = input.IsPublic,
                ImageUrl = imageUrl,
            };

            await this._postsRepository.AddAsync(post);
            await this._postsRepository.SaveChangesAsync();
        }

        public async Task Delete(string postId)
        {
            var post = this._postsRepository.All().FirstOrDefault(x => x.Id == postId);
            if (post == null)
            {
                return;
            }

            this._postsRepository.Delete(post);
            await this._postsRepository.SaveChangesAsync();
        }

        public ICollection<PostViewModel> AllPublic()
        {
            return this._postsRepository.AllAsNoTracking()
                .Where(x => x.IsPublic)
                .Select(x => new PostViewModel()
                {
                    Date = x.Date,
                    OwnerId = x.OwnerId,
                    OwnerUsername = x.Owner.UserName,
                    TextContent = x.TextContent,
                    ImageUrl = x.ImageUrl,
                    Comments = x.Comments.Select(c => new CommentViewModel()
                    {
                        Date = c.CreatedOn,
                        OwnerId = c.OwnerId,
                        OwnerName = c.Owner.UserName,
                        TextContent = c.TextContent,
                    }).OrderBy(x => x.Date).ToList(),
                    Likes = x.Likes.Select(l => new LikeViewModel()
                    {
                        OwnerId = l.OwnerId,
                        OwnerName = l.Owner.UserName,
                    }).ToList(),
                }).OrderByDescending(x => x.Date).ToList();
        }
    }
}
