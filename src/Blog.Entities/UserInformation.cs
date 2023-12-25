using Blog.Entities.Identity;

namespace Blog.Entities;
public class UserInformation : BaseEntity
{
    [MaxLength(512)]
    public string Biography { get; set; }

    [MaxLength(50)]
    public string Website { get; set; }

    [MaxLength(30)]
    public string Instagram { get; set; }

    [MaxLength(15)]
    public string Twitter { get; set; }

    [MaxLength(100)]
    public string Linkedin { get; set; }

    public int UserId { get; set; }

    #region Relations
    public User User { get; set; }
    #endregion
}
