using MyHomeLib.Database;
using System;

namespace ConsoleApp1
{
  public static class BookInfoFormatter
  {
    public static string ToFormattedString(this BookInfo b)
    {
      if (b == null) 
        throw new ArgumentNullException(nameof(b));
      return $"{b.AuthorLastName} {b.AuthorFirstName}: \n    {GetSeriesTitle(b.SeriesTitle)}{GetSeqNumber(b.SeqNumber)}{b.BookTitle}. {b.UpdateDate}. \\{b.Folder}\\{b.FileName + b.Ext}";
    }
    private static string GetSeqNumber(int value)
    {
      return value == 0 ? "" : value + ". ";
    }

    private static string GetSeriesTitle(string value)
    {
      return string.IsNullOrWhiteSpace(value) ? "" : value + " - ";
    }
  }
}
