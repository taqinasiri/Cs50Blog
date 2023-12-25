using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Blog.ViewModels.Post;

public class ShowListPostsViewModel
{
    public int Id { get; set; }
    [Display(Name = "Title")]
    public string Title { get; set; }
    [Display(Name = "Cover")]
    public string Cover { get; set; }
}
