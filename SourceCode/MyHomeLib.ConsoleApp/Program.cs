using MyHomeLib.Database;
using LibRusEc.FileStorage;
using System;
using System.IO;
using System.Linq;
using System.Configuration;

namespace MyHomeLib.ConsoleApp
{
  class Program
  {
    static void Main()
    {
      var processor = new BooksProcessor();
      processor.Execute();
    }
  }
}
