using System;
using uni.learn.core.Entity;

namespace uni.learn.core.Specifications;

public class CursoSpecifications : BaseSpecification<Curso>
{

    public CursoSpecifications(CursoSpecificationParams cursoParams) : base(
        x => (string.IsNullOrEmpty(cursoParams.Search) || x.Titulo.Contains(cursoParams.Search) || x.Descripcion.Contains(cursoParams.Search)) &&
            (string.IsNullOrEmpty(cursoParams.AutorId) || x.AuthorId == cursoParams.AutorId) &&
            (string.IsNullOrEmpty(cursoParams.Tema) || x.Temas.Any(t => t.Nombre.Contains(cursoParams.Tema)))
    )
    {

        AddInclude(x => x.Temas);
        AddOrderBy(x=>x.Vistas);
        ApplyPaging(cursoParams.PageSize * (cursoParams.PageIndex - 1), cursoParams.PageSize);


    }




}
