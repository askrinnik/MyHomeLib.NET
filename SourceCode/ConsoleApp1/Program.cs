using MyHomeLib.Database;
using MyHomeLib.FileStorage;
using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  class Program
  {
    static void Main()
    {
      MyCall2();
    }
    private static void MyCall2()
    {
      var bookName = "Блэка%";
      var repo = new DatabaseRepository("D:\\Downloads\\librusec_local_fb2.hlc2");
      var books = repo.GetBooksByTitle(bookName);
      foreach (var b in books)
        Console.WriteLine($"{b.AuthorFirstName} {b.AuthorLastName}: {b.SeriesTitle} - {b.BookTitle}");

      var book = books.First();
      string fileName = book.FileName + book.Ext;

      //var storageRepo = new FileStorageRepository(@"E:\Disk_D\_Lib.rus.ec - Официальная\Lib.rus.ec");
      var storageRepo = new FileStorageRepository(@"D:\_Lib.rus.ec - Официальная\Lib.rus.ec");
      using var stream = storageRepo.GetFile(book.Folder, fileName);
      if(stream != null)
      {
        using var fileStream = File.Create(Path.Combine(@"D:\Downloads", $"{book.Folder}_{fileName}"));
        stream.CopyTo(fileStream);
      }
    }

  }
}
