using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace MyHomeLib.Database
{
  internal static class SqliteExtensions
  {
    public static List<T> ReadData<T>(this SqliteCommand stmt, Func<SqliteDataReader, T> getElement) 
    {
      if (stmt == null)
        throw new ArgumentNullException(nameof(stmt));

      var list = new List<T>();
      using (var rdr = stmt.ExecuteReader())
        while (rdr.Read())
        {
          var element = getElement(rdr);
          if (element != null)
            list.Add(element);
        }
      return list;
    }
    public static string GetString(this SqliteDataReader reader, string fieldName)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof(reader));

      int ordinal = reader.GetOrdinal(fieldName);
      return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }
    public static int GetInt32(this SqliteDataReader reader, string fieldName)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof(reader));
      int ordinal = reader.GetOrdinal(fieldName);
      return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(reader.GetOrdinal(fieldName));
    }

  }
}
