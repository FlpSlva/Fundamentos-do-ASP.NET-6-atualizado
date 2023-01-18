using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Pasword { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O E-mail é inválido")]
        public string Email { get; set; }

    }
}
