using Blog.Entities;
using Blog.ViewModels.Post;

namespace Blog.Services.Contracts;

public interface IPostService : IGenericService<Post>
{
    Task<List<ShowListPostsViewModel>> GetPostsListAsync();
    Task<PostDetailViewModel> GetPostDetailsAsync(int postId);
    Task<List<ShowPostsViewModel>> GetPostsAsync();
    Task<EditPostViewModel> GetPostForEditAsync(int postId);
}
