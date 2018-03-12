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

         // JSON
         var generateJson = bool.Parse(settings.Get("GenerateJson"));
         var jsonFilePath = settings.Get("JsonPath");
         var jsonFileName = settings.Get("JsonFileName");
         var compressJson = bool.Parse(settings.Get("CompressJson"));


         if (generateXml)
         {
            var xmlGenerator = new XmlGenerator(xmlFileName);
            xmlGenerator.Generate(sourcePath, xmlPath);

            var fileGenerator = new FileGenerator();
            fileGenerator.GenerateFromXml(Path.Combine(xmlPath, xmlFileName), targetPath);

            if (!compressXml) return;
            var compressor = new Compressor(Path.Combine(xmlPath, xmlFileName));
            compressor.Compress();
         }

         if (generateJson)
         {
            var jsonGenerator = new JsonGenerator(jsonFileName);
            jsonGenerator.Generate(sourcePath, jsonFilePath);

            var fileGenerator = new FileGenerator();
            fileGenerator.GenerateFromJson(Path.Combine(jsonFilePath, jsonFileName), targetPath);

            if (!compressJson) return;
            var compressor = new Compressor(Path.Combine(jsonFilePath, jsonFileName));
            compressor.Compress();
         }
      }
   }
}