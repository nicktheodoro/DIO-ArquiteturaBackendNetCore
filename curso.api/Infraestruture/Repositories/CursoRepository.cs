using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Infraestruture.Data;
using Microsoft.EntityFrameworkCore;

namespace curso.api.Infraestruture.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoDbContext _contexto;

        public CursoRepository(CursoDbContext contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(Curso curso)
        {
            _contexto.Curso.Add(curso);
        }
        
        public void Commit()
        {
            _contexto.SaveChanges();
        }
        public IList<Curso> ObterPorUsuario(int codigoUsuario)
        {
            // Include aproveita a fk e faz um Inner join entre Usuario e Curso
            return _contexto.Curso.Include(i => i.Usuario).Where(w => w.CodigoUsuario == codigoUsuario).ToList();
        }
    }
}