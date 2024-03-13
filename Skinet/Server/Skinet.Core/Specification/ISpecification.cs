﻿using System.Linq.Expressions;

namespace Skinet.Core.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        ICollection<Expression<Func<T, object>>> Includes { get;}

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        int Take { get; }

        int Skip { get; }

        bool isPagingEnabled { get; }

    }
}
