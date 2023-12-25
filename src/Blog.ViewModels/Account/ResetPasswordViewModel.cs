using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Display(Prompt = "Email")]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
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

        [HiddenInput]
        public string Token { get; set; }
    }
}
