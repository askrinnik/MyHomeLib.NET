using System;

namespace MyHomeLib.Database
{
  public class BookInfo: IComparable
  {
    public string SeriesTitle { get; internal set; }
    public string AuthorFirstName { get; internal set; }
    public string AuthorLastName { get; internal set; }
    public string BookTitle { get; internal set; }
    public int ID { get; internal set; }
    public string UpdateDate { get; internal set; }
    public string Folder { get; internal set; }
    public string FileName { get; internal set; }
    public string Ext { get; internal set; }
    public int BookSize { get; internal set; }
    public bool IsDeleted { get; internal set; }
    public int SeqNumber { get; internal set; }

    public int CompareTo(BookInfo other)
    {
      if (other == null)
        return -1;

      var result = string.Compare(AuthorLastName, other.AuthorLastName, StringComparison.InvariantCulture);
      if (result != 0)
        return result;

      result = string.Compare(AuthorFirstName, other.AuthorFirstName, StringComparison.InvariantCulture);
      if (result != 0)
        return result;

      result = string.Compare(SeriesTitle, other.SeriesTitle, StringComparison.InvariantCulture);
      if (result != 0)
        return result;

      result = SeqNumber.CompareTo(other.SeqNumber);
      if (result != 0)
        return result;

      result = string.Compare(BookTitle, other.BookTitle, StringComparison.InvariantCulture);
      return result != 0 ? result : 0;
    }

    public int CompareTo(object obj)
    {
      return CompareTo(obj as BookInfo);
    }

    public override bool Equals(object obj)
    {
      return obj is BookInfo info &&
             AuthorLastName == info.AuthorLastName &&
             AuthorFirstName == info.AuthorFirstName &&
             SeriesTitle == info.SeriesTitle &&
             SeqNumber == info.SeqNumber &&
             BookTitle == info.BookTitle;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(AuthorLastName, AuthorFirstName, SeriesTitle, SeqNumber, BookTitle);
    }

    public static bool operator ==(BookInfo left, BookInfo right)
    {
      return left is null ? right is null : left.Equals(right);
    }

    public static bool operator !=(BookInfo left, BookInfo right)
    {
      return !(left == right);
    }

    public static bool operator <(BookInfo left, BookInfo right)
    {
      return left is null ? right is object : left.CompareTo(right) < 0;
    }

    public static bool operator <=(BookInfo left, BookInfo right)
    {
      return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(BookInfo left, BookInfo right)
    {
      return left is object && left.CompareTo(right) > 0;
    }

    public static bool operator >=(BookInfo left, BookInfo right)
    {
      return left is null ? right is null : left.CompareTo(right) >= 0;
    }
  }
}
