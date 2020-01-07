using System;
using System.IO;
using System.IO.Compression;

namespace MyHomeLib.FileStorage
{
  public class FileStorageRepository
  {
    private readonly string storageFolder;

    public FileStorageRepository(string storageFolder)
    {
      this.storageFolder = storageFolder;
    }

    public Stream GetFile(string archiveName, string fileName)
    {
      using var za = ZipFile.OpenRead(Path.Combine(storageFolder, archiveName));
      foreach (var entry in za.Entries)
      {
        if (entry.Name != fileName) continue;

        using var stream = entry.Open();
        var ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0; // rewind
        return ms;
      }
      return null;
    }
  }
}
