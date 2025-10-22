using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Framework.Paginations
{
    public static class Pagination
    {
        public static IEnumerable<T> ToPaged<T>(this IEnumerable<T> source, int pageNum, int pageSize, out int rowsCount)
        {
            rowsCount = source.Count();
            return source.Skip((pageNum - 1) * pageSize).Take(pageSize);
        }

        public static int PageCount( int rowsCount, int pageSize)
        {
            var res = Math.Ceiling(rowsCount / (decimal)pageSize);
            return Convert.ToInt32(res);
        }
    }
}
