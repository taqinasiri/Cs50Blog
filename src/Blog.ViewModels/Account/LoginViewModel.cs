using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blog.ViewModels.Account
{
    public class LoginViewModel
    {

        [Display(Prompt = "UserName")]
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [Display(Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public List<AuthenticationScheme> ExternalLogins{ get; set; }
    }
}
