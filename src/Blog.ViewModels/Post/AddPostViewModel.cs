using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Blog.ViewModels.Post;

public class AddPostViewModel
{
    [Required]
    [MaxLength(100)]
    [Display(Prompt = "Title")]
    public string Title { get; set; }

    [Required]
    [Display(Prompt = "Content")]
    public string Content { get; set; }
    [Display(Name = "Cover")]
    public IFormFile Cover { get; set; }
}
