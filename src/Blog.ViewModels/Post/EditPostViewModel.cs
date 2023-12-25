using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.ViewModels.Post;

public class EditPostViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Prompt = "Title")]
    public string Title { get; set; }

    [Required]
    [Display(Prompt = "Content")]
    public string Content { get; set; }
    [Display(Name = "Cover")]
    public IFormFile Cover { get; set; }

    public string ThisCover { get; set; }
}
