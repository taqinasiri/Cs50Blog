using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Prompt = "UserName")]
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [RegularExpression(@"^\w+$", ErrorMessage = "Only English characters are allowed")]
        [Remote("CheckUserName", "Account", null,HttpMethod = "POST",AdditionalFields = ViewModelConstants.AntiForgeryToken)]
        public string UserName { get; set; }

        [Display(Prompt = "Email")]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [Remote("CheckEmail", "Account", null,HttpMethod = "POST",AdditionalFields = ViewModelConstants.AntiForgeryToken)]
        public string Email { get; set; }

        [Display(Prompt = "Password")]
        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Prompt = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
