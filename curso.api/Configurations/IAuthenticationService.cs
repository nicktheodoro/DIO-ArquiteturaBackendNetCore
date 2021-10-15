using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using curso.api.Models.Usuarios;

namespace curso.api.Configurations
{
    public interface IAuthenticationService
    {
        string GerarToken(UsuarioViewModelOutput usuarioViewModelOutput);
    }
}