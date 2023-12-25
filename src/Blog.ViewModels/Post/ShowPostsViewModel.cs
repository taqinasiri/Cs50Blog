using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Blog.ViewModels.Post;

public class ShowPostsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Cover { get; set; }
    public string PreviewContent { get; set; }
}
