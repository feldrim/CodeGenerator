using System.IO;
using System.IO.Compression;

namespace CodeGenerator
{
   public class Compressor
   {
      private readonly FileInfo _fileInfo;

      public Compressor(string source)
      {
         _fileInfo = new FileInfo(source);
      }


      public void Compress()
      {
         // Get the stream of the source file.
         using (var inFile = _fileInfo.OpenRead())
         {
            // Prevent compressing hidden and already compressed files.
            if (!(((File.GetAttributes(_fileInfo.FullName)
                    & FileAttributes.Hidden)
                   != FileAttributes.Hidden) & (_fileInfo.Extension != ".gz"))) return;

            // Create the compressed file.
            using (var outFile =
               File.Create(_fileInfo.FullName + ".gz"))
            {
               using (var compress =
                  new GZipStream(outFile,
                     CompressionMode.Compress))
               {
                  // Copy the source file into the compression stream.
                  inFile.CopyTo(compress);
               }
            }
         }
      }
   }
}