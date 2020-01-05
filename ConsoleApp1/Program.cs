using Microsoft.Data.Sqlite;
using MyHomeLib.DAL;
using System;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      MyCall2();
    }
    private static void MyCall2()
    {
      var bookName = "Башня ласточки%";
      var repo = new MHLRepository("librusec_local_fb2.hlc2");
      var books = repo.GetBooksByName(bookName);
      foreach (var book in books)
      {
        Console.WriteLine($"SeriesTitle: {book.SeriesTitle}");
      }
    }

    private static void MyCall()
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
