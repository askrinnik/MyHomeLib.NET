using System.Collections.Generic;

namespace MyHomeLib.Database
{
  public interface IDatabaseRepository
  {
    IEnumerable<BookInfo> GetBooksByTitle(string titlePart);
    IEnumerable<BookInfo> GetBooksBySerie(string titlePart);
  }
}