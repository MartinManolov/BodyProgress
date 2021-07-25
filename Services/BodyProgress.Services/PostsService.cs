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
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IUploadMediaService _uploadMediaService;
        private readonly IUsersService usersService;
        private readonly Cloudinary cloudinary;

        public PostsService(IDeletableEntityRepository<Post> postsRepository,
            IUploadMediaService uploadMediaService,
            IUsersService usersService,
            Cloudinary cloudinary)
        {
            this.postsRepository = postsRepository;
            this._uploadMediaService = uploadMediaService;
            this.usersService = usersService;
            this.cloudinary = cloudinary;
        }

        public async Task Create(PostInputModel input, string userId)
        {
            var imageName = userId + Guid.NewGuid().ToString();
            var imageUrl = await this._uploadMediaService.UploadImage(this.cloudinary, input.Image, imageName);
            var post = new Post()
            {
                Date = DateTime.UtcNow,
                OwnerId = userId,
                TextContent = input.TextContent,
                IsPublic = input.IsPublic,
                ImageUrl = imageUrl,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task Delete(string postId)
        {
            var post = this.postsRepository.All().FirstOrDefault(x => x.Id == postId);
            if (post == null)
            {
                return;
            }

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public ICollection<PostViewModel> AllPublicAndFriends(string userId)
        {
            return this.postsRepository.AllAsNoTracking()
                .Where(x => x.IsPublic || x.Owner.FriendRequestsAccepted
                                            .Any(f => f.UserId == userId &&
                                            f.FriendId == x.OwnerId && f.Status == Data.Models.Enums.FriendshipStatus.Friends) ||
                                            x.Owner.FriendRequestsMade
                                            .Any(f => f.UserId == x.OwnerId && f.FriendId == userId && f.Status == Data.Models.Enums.FriendshipStatus.Friends))
                .Select(x => new PostViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    IsLiked = x.Likes.Any(l => l.OwnerId == userId),
                    OwnerUsername = x.Owner.UserName,
                    OwnerProfilePicture = this.usersService.GetProfileImage(x.OwnerId),
                    TextContent = x.TextContent,
                    ImageUrl = x.ImageUrl,
                    Comments = x.Comments.Select(c => new CommentViewModel()
                    {
                        Date = c.CreatedOn,
                        OwnerName = c.Owner.UserName,
                        TextContent = c.TextContent,
                    }).OrderBy(x => x.Date).ToList(),
                    Likes = x.Likes.Select(l => new LikeViewModel()
                    {
                        Username = l.Owner.UserName,
                    }).ToList(),
                }).OrderByDescending(x => x.Date).ToList();
        }
    }
}
