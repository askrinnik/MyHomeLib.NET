using Microsoft.Data.Sqlite;
using MyHomeLib.Database;
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
      var repo = new DatabaseRepository("D:\\Downloads\\librusec_local_fb2.hlc2");
      var books = repo.GetBooksByTitle(bookName);
      foreach (var book in books)
      {
        Console.WriteLine($"{book.AuthorFirstName} {book.AuthorLastName}: {book.SeriesTitle} - {book.BookTitle}");
      }
    }

  }
}
