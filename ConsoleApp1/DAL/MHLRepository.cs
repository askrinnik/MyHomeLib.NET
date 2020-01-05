using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHomeLib.DAL
{
  public class MHLRepository
  {
    private readonly string dbFilePath;

    public MHLRepository(string dbFilePath)
    {
      this.dbFilePath = dbFilePath;
    }

    public IEnumerable<BookInfo> GetBooksByName(string bookNamePart)
    {
      const string query = @"select s.SeriesTitle, a.FirstName, a.LastName, b.BookID, b.Title, b.UpdateDate, b.Folder, b.FileName, b.BookSize, b.IsDeleted from Books b
        join Series s on s.SeriesID = b.SeriesID
        join Author_List al on al.BookID = b.BookID
        join Authors a on a.AuthorID = al.AuthorID
        where Title like $name";
      return ExecuteCommand(command =>
        {
          command.CommandText = query;
          command.Parameters.AddWithValue("$name", bookNamePart);
        }, 
        reader =>
          new BookInfo
          {
            SeriesTitle = reader.GetString("SeriesTitle"),
            AuthorFirstName = reader.GetString("FirstName"),
            AuthorLastName = reader.GetString("LastName"),
            BookID = reader.GetInt32("BookID"),
            BookTitle = reader.GetString("Title"),
            UpdateDate = reader.GetString("UpdateDate"),
            Folder = reader.GetString("Folder"),
            FileName = reader.GetString("FileName"),
            BookSize = reader.GetInt32("BookSize"),
            IsDeleted = reader.GetInt32("IsDeleted")==1,
          });
    }

    private T ExecuteCommand<T>(Func<SqliteCommand, T> getResult)
    {
      using var connection = new SqliteConnection($"Data Source={dbFilePath}");
      connection.Open();
      using var command = connection.CreateCommand();
      return getResult(command);
    }

    private IEnumerable<T> ExecuteCommand<T>(Action<SqliteCommand> prepareCommand, Func<SqliteDataReader, T> getElement)
    {
      return ExecuteCommand<IEnumerable<T>>(command =>
      {
        prepareCommand(command);
        var data = command.ReadData(getElement);
        return data;
      });
    }


  }
}
