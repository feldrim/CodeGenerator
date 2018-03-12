using System;
using System.IO;
using System.Xml;

namespace CodeGenerator
{
   public class XmlGenerator
   {
      private readonly string _fileName;
      private readonly XmlDocument _xmlDocument;

      public XmlGenerator(string fileName)
      {
         _xmlDocument = new XmlDocument();
         _fileName = fileName;
      }

      public void Generate(string solutionPath, string xmlPath)
      {
         if (File.Exists(xmlPath)) return;
         GenerateXmlFromFiles(new DirectoryInfo(solutionPath), _xmlDocument);
         Save(xmlPath);
      }

      private void GenerateXmlFromFiles(FileSystemInfo directoryInfo, XmlNode rootNode)
      {
         var directoryNode = _xmlDocument.CreateElement("Directory");
         directoryNode.SetAttribute("DirectoryName", directoryInfo.Name);
         rootNode.AppendChild(directoryNode);

         // File
         var files = Directory.GetFiles(directoryInfo.FullName);
         foreach (var file in files)
         {
            var fileName = new FileInfo(file).Name;
            var bytes = File.ReadAllBytes(file);
            var base64 = Convert.ToBase64String(bytes);
            var fileNode = _xmlDocument.CreateElement("File");
            fileNode.SetAttribute("FileName", fileName);
            fileNode.InnerText = base64;

            directoryNode.AppendChild(fileNode);
         }

         // Recursive directory exploration
         var directories = Directory.GetDirectories(directoryInfo.FullName);

         foreach (var directory in directories) GenerateXmlFromFiles(new DirectoryInfo(directory), directoryNode);
      }
      
      private void Save(string path)
      {
         _xmlDocument.Save(Path.Combine(path, _fileName));
      }
   }
}