using curso.api.Business.Entities;

namespace curso.api.Business.Repositories
{
    public interface IUsuarioRepository
    {
        void Adcionar(Usuario usuario);
        void Commit();
        Usuario Obter(string login);
    }
}
