using System.Linq;
using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Infraestruture.Data;

namespace curso.api.Infraestruture.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CursoDbContext _contexto;

        public UsuarioRepository(CursoDbContext contexto)
        {
            _contexto = contexto;
        }

        public void Adcionar(Usuario usuario)
        {
            _contexto.Usuario.Add(usuario);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public Usuario Obter(string login)
        {
            return _contexto.Usuario.FirstOrDefault(u => u.Login == login);
        }
    }
}
