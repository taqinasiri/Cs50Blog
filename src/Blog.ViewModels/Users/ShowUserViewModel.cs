using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Users;

    public class ShowUserViewModel
    {
        public int Id { get; set; }
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

    [Display(Name = "Register date")]
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }