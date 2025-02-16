
using Microsoft.EntityFrameworkCore;
using uni.learn.core.Entity;
using uni.learn.core.Specifications;

namespace uni.learn.BussinesLogic.Context.Data;

public class SpecificationEvaluator<T> where T : Base
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec){
        if(spec.Criteria != null){
            inputQuery = inputQuery.Where(spec.Criteria);
        }

        if(spec.IsPagingEnabled){
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
        }


        inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));


        return inputQuery;
    }

}
