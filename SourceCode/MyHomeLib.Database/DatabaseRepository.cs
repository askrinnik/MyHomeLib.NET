using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace MyHomeLib.Database
{
  public class DatabaseRepository : IDatabaseRepository
  {
    private readonly string dbFilePath;

    public DatabaseRepository(string dbFilePath)
    {
      this.dbFilePath = dbFilePath;
    }

    public IEnumerable<BookInfo> GetBooksByTitle(string titlePart)
    {
      const string query = @"select s.SeriesTitle, a.FirstName, a.LastName, b.BookID, b.Title, b.UpdateDate, b.Folder, b.FileName, b.Ext, b.BookSize, b.IsDeleted from Books b
        join Series s on s.SeriesID = b.SeriesID
        join Author_List al on al.BookID = b.BookID
        join Authors a on a.AuthorID = al.AuthorID
        where SearchTitle like $title";

      return ExecuteCommand(command =>
        {
          command.CommandText = query;
          command.Parameters.AddWithValue("$title", titlePart.ToUpperInvariant());
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
            Ext = reader.GetString("Ext"),
            BookSize = reader.GetInt32("BookSize"),
            IsDeleted = reader.GetInt32("IsDeleted") == 1,
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

    public static void MyCall()
    {
      var bookName = "Башня ласточки%";
      using var connection = new SqliteConnection("Data Source=librusec_local_fb2.hlc2");
      connection.Open();
      using var command = connection.CreateCommand();
      command.CommandText =
      @"select s.SeriesTitle, a.FirstName, a.LastName, b.Title,  b.* from Books b
join Series s on s.SeriesID = b.SeriesID
join Author_List al on al.BookID = b.BookID
join Authors a on a.AuthorID = al.AuthorID
where Title like $name";
      command.Parameters.AddWithValue("$name", bookName);

      using var reader = command.ExecuteReader();
      while (reader.Read())
      {
        var SeriesTitle = reader.GetString(0);

        Console.WriteLine($"SeriesTitle: {SeriesTitle}");
      }

    }


  }
}
