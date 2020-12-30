using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FluentValidation.Sample.Extension
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> query, bool flag, Expression<Func<T, bool>> expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return flag ? query.Where(expression) : query;
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, string flag, Expression<Func<T, bool>> expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return string.IsNullOrEmpty(flag) ? query.Where(expression) : query;
        }
    }
}
