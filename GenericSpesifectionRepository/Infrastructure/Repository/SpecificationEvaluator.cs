using GenericSpesifectionRepositoryApp.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GenericSpesifectionRepositoryApp.Infrastructure.Repository;

public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        var query = inputQuery;

        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);

        if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);

        if (specification.GroupBy != null)
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        if (specification.IsPagingEnabled && specification.Skip.HasValue && specification.Take.HasValue)
            query = query.Skip(specification.Skip.Value).Take(specification.Take.Value);


        return query;
    }
}