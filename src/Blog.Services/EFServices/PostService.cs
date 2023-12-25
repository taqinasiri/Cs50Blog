using Blog.DataLayer.Context;
using Blog.Entities;
using Blog.Services.Contracts;
using Blog.ViewModels.Post;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.EFServices;

public class PostService : GenericService<Post>, IPostService
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<Post> _post;

    public PostService(IUnitOfWork uow) : base(uow)
    {
        _uow = uow;
        _post = _uow.Set<Post>();

    }

    public Task<List<ShowListPostsViewModel>> GetPostsListAsync() => _post
        .Select(p => new ShowListPostsViewModel()
        {
            Id = p.Id,
            Title = p.Title,
            Cover = p.CoverImage
        }).ToListAsync();

    public Task<PostDetailViewModel> GetPostDetailsAsync(int postId) => _post
        .Select(p => new PostDetailViewModel()
        {
            Content = p.Content,
            Title = p.Title,
            Cover= p.CoverImage,
            Id = p.Id
        }).SingleOrDefaultAsync(p => p.Id == postId);

    public Task<List<ShowPostsViewModel>> GetPostsAsync() => _post
        .Select(p => new ShowPostsViewModel()
        {
            Id = p.Id,
            Title = p.Title,
            Cover = p.CoverImage,
            PreviewContent = p.Content.Substring(0,100)
        }).ToListAsync();
    public Task<EditPostViewModel> GetPostForEditAsync(int postId) => _post
        .Select(p => new EditPostViewModel()
        {
            Content = p.Content,
            Id = p.Id,
            Title = p.Title,
            ThisCover = p.CoverImage
        }).SingleOrDefaultAsync(p => p.Id == postId);
}
