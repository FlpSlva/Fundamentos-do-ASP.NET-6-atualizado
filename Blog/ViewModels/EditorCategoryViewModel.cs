using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required( ErrorMessage = "O nome é obrigatório !")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter 3 e 40 caracteres")]
        public string Name { get; private set; }

        [Required(ErrorMessage = "O slug é obrigatório !")]
        public string Slug { get; private set; }
    }
}
