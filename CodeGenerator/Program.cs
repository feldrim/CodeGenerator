using System.Configuration;
using System.IO;
using System.Linq;

namespace CodeGenerator
{
   public static class Program
   {
      public static void Main(string[] args)
      {
         var settings = ConfigurationManager.AppSettings;
         var sourcePath = settings.Get("Source");
         var targetPath = settings.Get("Target");

         // XML
         var generateXml = bool.Parse(settings.Get("GenerateXml"));
         var xmlPath = settings.Get("XmlPath");
         var xmlFileName = settings.Get("XmlFileName");
         var compressXml = bool.Parse(settings.Get("CompressXml"));

         // File
         var generateFile = bool.Parse(settings.Get("GenerateFile"));

         if (generateXml)
         {
            var xmlGenerator = new XmlGenerator(xmlFileName);
            xmlGenerator.Generate(sourcePath, xmlPath);

            if (compressXml)
            {
               var compressor = new Compressor(Path.Combine(xmlPath, xmlFileName));
               compressor.Compress();
            }
         }

         if (generateFile)
         {
            var fileGenerator = new FileGenerator();
            fileGenerator.GenerateFromXml(Path.Combine(xmlPath, xmlFileName), targetPath);
         }
      }
   }
}