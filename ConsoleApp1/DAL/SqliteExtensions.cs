using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace MyHomeLib.DAL
{
  public static class SqliteExtensions
  {
    public static List<T> ReadData<T>(this SqliteCommand stmt, Func<SqliteDataReader, T> getElement)
    {
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
      return reader.GetString(reader.GetOrdinal(fieldName));
    }
    public static int GetInt32(this SqliteDataReader reader, string fieldName)
    {
      return reader.GetInt32(reader.GetOrdinal(fieldName));
    }

  }
}
