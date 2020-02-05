using MyHomeLib.Database;
using LibRusEc.FileStorage;
using System;
using System.IO;
using System.Linq;
using System.Configuration;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      var bookName = args.Any() ? args[0] : "%оборотень%";
      MyCall(bookName);
    }
    private static void MyCall(string bookName)
    {
      //var dbPath = @"D:\New\librusec_local_fb2.hlc2";
      //var storagePath = @"D:\_Lib.rus.ec - Официальная\Lib.rus.ec";
      //var dbPath = @"D:\Downloads\librusec_local_fb2.hlc2";
      //var storagePath = @"E:\Disk_D\_Lib.rus.ec - Официальная\Lib.rus.ec";
      var dbPath = ConfigurationManager.AppSettings["dbPath"];
      var storagePath = ConfigurationManager.AppSettings["storagePath"]; ;

      var books = FindBooks(bookName, dbPath);
      PrintBooks(books);
      SaveBooks(storagePath, books);
    }

    private static BookInfo[] FindBooks(string bookName, string dbPath)
    {
      var repo = new DatabaseRepository(dbPath);
      var books = repo.GetBooksByTitle(bookName).OrderBy(b => b).ToArray();
      Console.WriteLine($"Books found: {books.Length}");
      return books;
    }

    private static void SaveBooks(string storagePath, BookInfo[] books)
    {
      while (true)
      {
        var bookIndex = GetBookIndex(books.Length + 1);
        if (bookIndex == 0)
          break;
        SaveBook(storagePath, books[bookIndex - 1]);
      }
    }

    private static void PrintBooks(BookInfo[] books)
    {
      int index = 1;
      foreach (var b in books)
        Console.WriteLine($"{index++}. " + b.ToFormattedString());
    }

    private static void SaveBook(string storagePath, BookInfo book)
    {
      string fileName = book.FileName + book.Ext;

      var storageRepo = new FileStorageRepository(storagePath);
      using var stream = storageRepo.GetFile(book.Folder, fileName);
      if (stream != null)
      {
        string path = Path.Combine(@"D:\", $"{book.Folder}_{fileName}");
        using var fileStream = File.Create(path);
        stream.CopyTo(fileStream);
        Console.WriteLine($"The book has been saved to {path}.");
      }
    }

    private static int GetBookIndex(int maxValidValue)
    {
      while (true)
      {
        Console.WriteLine("Enter book index for download or 'q' for exit");
        var value = Console.ReadLine();

        if (value == "q") return 0; 

        if (!int.TryParse(value, out int bookIndex))
        {
          Console.WriteLine("Invalid value");
          continue;
        }
        if (bookIndex > maxValidValue || bookIndex <= 0)
        {
          Console.WriteLine("Invalid book index");
          continue;
        }
        return bookIndex;
      }
    }
  }
}
