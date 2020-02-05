using LibRusEc.FileStorage;
using MyHomeLib.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MyHomeLib.ConsoleApp
{
  class BooksProcessor
  {
    enum SearchType { ByBookTitle, BySeriesTitle, Exit };

    public void Execute()
    {
      while (true)
      {
        var searchType = GetSearchType();
        if (searchType == SearchType.Exit)
          break;

        var stringValue = GetStringValue($"Enter a part of a {(searchType == SearchType.ByBookTitle ? "book" : "serie")} title (at least 5 chars). % can be used. ");
        if (stringValue == null) 
          break;

        var books = FindBooks(searchType, stringValue);
        PrintBooks(books);
        SaveBooks(books);
      }
    }
    private static SearchType GetSearchType()
    {
      while (true)
      {
        Console.WriteLine("Choose search type:\n  1: By book title.\n  2: By serie title\n  'q' for exit");
        var value = Console.ReadLine();

        if (value == "q") return SearchType.Exit;

        if (!int.TryParse(value, out int menuIndex) || menuIndex > 2 || menuIndex <= 0)
        {
          Console.WriteLine("Invalid value");
          continue;
        }
        return menuIndex == 1 ? SearchType.ByBookTitle : SearchType.BySeriesTitle;
      }
    }

    private static string GetStringValue(string message)
    {
      while (true)
      {
        Console.WriteLine(message + " Enter 'q' for exit");
        var value = Console.ReadLine();

        if (value == "q") return null;

        if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
        {
          Console.WriteLine("Invalid value");
          continue;
        }
        return value;
      }
    }

    private static BookInfo[] FindBooks(SearchType searchType, string bookName)
    {
      //var dbPath = @"D:\New\librusec_local_fb2.hlc2";
      //var dbPath = @"D:\Downloads\librusec_local_fb2.hlc2";
      var dbPath = ConfigurationManager.AppSettings["dbPath"];

      var repo = new DatabaseRepository(dbPath);
      var books = (searchType == SearchType.ByBookTitle ? repo.GetBooksByTitle(bookName) : repo.GetBooksBySerie(bookName))
        .OrderBy(b => b).ToArray();
      Console.WriteLine($"Books found: {books.Length}");
      return books;
    }

    private static void PrintBooks(BookInfo[] books)
    {
      int index = 1;
      foreach (var b in books)
        Console.WriteLine($"{index++}. " + b.ToFormattedString());
    }

    private static void SaveBooks(BookInfo[] books)
    {
      while (true)
      {
        var bookIndex = GetBookIndex(books.Length + 1);
        if (bookIndex == 0)
          break;
        SaveBook(books[bookIndex - 1]);
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

    private static void SaveBook(BookInfo book)
    {
      //var storagePath = @"D:\_Lib.rus.ec - Официальная\Lib.rus.ec";
      //var storagePath = @"E:\Disk_D\_Lib.rus.ec - Официальная\Lib.rus.ec";
      var storagePath = ConfigurationManager.AppSettings["storagePath"];

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

  }
}
