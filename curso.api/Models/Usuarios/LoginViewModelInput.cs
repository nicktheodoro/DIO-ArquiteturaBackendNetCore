using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Usuarios
{
    public class LoginViewModelInput
    {
        [Required(ErrorMessage = "O campo login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatória")]
        public string Senha { get; set; }

    }
}
