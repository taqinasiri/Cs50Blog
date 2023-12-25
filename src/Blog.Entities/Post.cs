using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    [Required]
    [Column(TypeName = "ntext")]
    public string Content { get; set; }
    public string CoverImage { get; set; }
    public DateTime CreateDate { get; set; }
}

