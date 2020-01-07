using System;
using System.Collections.Generic;
using System.Text;

namespace MyHomeLib.DAL
{
  public class BookInfo
  {
    public string SeriesTitle { get; internal set; }
    public string AuthorFirstName { get; internal set; }
    public string AuthorLastName { get; internal set; }
    public string BookTitle { get; internal set; }
    public int BookID { get; internal set; }
    public string UpdateDate { get; internal set; }
    public string Folder { get; internal set; }
    public string FileName { get; internal set; }
    public int BookSize { get; internal set; }
    public bool IsDeleted { get; internal set; }
  }
}
