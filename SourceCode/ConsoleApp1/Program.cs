using MyHomeLib.Database;
using MyHomeLib.FileStorage;
using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      var bookName = args.Any() ? args[0] : "БЛЭКАУТ";
      MyCall2(bookName);
    }
    private static void MyCall2(string bookName)
    {
      //var dbPath = @"D:\New\librusec_local_fb2.hlc2";
      //var storagePath = @"D:\_Lib.rus.ec - Официальная\Lib.rus.ec";
      var dbPath = @"D:\Downloads\librusec_local_fb2.hlc2";
      var storagePath = @"E:\Disk_D\_Lib.rus.ec - Официальная\Lib.rus.ec";


      var repo = new DatabaseRepository(dbPath);
      var books = repo.GetBooksByTitle(bookName);
      Console.WriteLine($"Books found: {books.Count()}");

      foreach (var b in books)
        Console.WriteLine($"{b.AuthorFirstName} {b.AuthorLastName}: {b.SeriesTitle} - {b.BookTitle}");

      var book = books.First();
      string fileName = book.FileName + book.Ext;

      //var storageRepo = new FileStorageRepository();
      var storageRepo = new FileStorageRepository(storagePath);
      using var stream = storageRepo.GetFile(book.Folder, fileName);
      if(stream != null)
      {
        using var fileStream = File.Create(Path.Combine(@"D:\", $"{book.Folder}_{fileName}"));
        stream.CopyTo(fileStream);
      }
    }

  }
}
