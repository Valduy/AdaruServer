using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DBRepository.Helpers
{
    public static class IDataReaderExtension
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader,
            Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}
