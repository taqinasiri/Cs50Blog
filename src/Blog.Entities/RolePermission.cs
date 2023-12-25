using Blog.Entities.Identity;

namespace Blog.Entities;
public class RolePermission : BaseEntity
{
    [MaxLength(255)]
    public string Permission { get; set; }

    public int RoleId { get; set; }

    #region Relations

    public Role Role { get; set; }
    #endregion
}
