using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace CodeGenerator
{
   public class FileGenerator
   {
      public void GenerateFromXml(string xmlPath, string outputPath)
      {
         var xmlDocument = new XmlDocument();
         xmlDocument.Load(xmlPath);
         var rootNode = xmlDocument.ChildNodes.Item(0);
         GenerateFromXml(rootNode, outputPath);
      }

      public void GenerateFromJson(string jsonPath, string outputPath)
      {
         var jsonDocument = JObject.Parse(File.ReadAllText(jsonPath));
         var rootNode = jsonDocument.First;
         GenerateFromJson(rootNode, outputPath);
      }

      private void GenerateFromJson(JToken node, string path)
      {
         if (node.HasValues) //directory
         {
            GenerateDirectoryFromJson(node, path);
            
            var newPath = Path.Combine(path, node.Path);
            var childNodes = node.Children();
            foreach (var childNode in childNodes)
            {
               GenerateFromJson(childNode, newPath);
            }
         }
         else
         {
            GenerateFileFromJson(node, path);
         }
      }

      private void GenerateDirectoryFromJson(JToken node, string path)
      {
         var newPath = Path.Combine(path, node.Path);
         Directory.CreateDirectory(newPath);
      }

      private void GenerateFileFromJson(JToken node, string path)
      {
         var newPath = Path.Combine(path, node.Path);

         var base64 = node.Value<string>();
         var bytes = Convert.FromBase64String(base64);

         File.WriteAllBytes(newPath, bytes);
      }

      private void GenerateFromXml(XmlNode node, string path)
      {
         if (node.Name.Equals("Directory"))
         {
            GenerateDirectoryFromXml(node, path);

            var newPath = Path.Combine(path, node.Attributes.GetNamedItem("DirectoryName").Value);
            var childNodes = node.ChildNodes;
            foreach (XmlNode childNode in childNodes) GenerateFromXml(childNode, newPath);
         }
         else if (node.Name.Equals("File"))
         {
            GenerateFileFromXml(node, path);
         }
      }

      private void GenerateDirectoryFromXml(XmlNode node, string path)
      {
         var newPath = Path.Combine(path, node.Attributes.GetNamedItem("DirectoryName").Value);
         Directory.CreateDirectory(newPath);
      }

      private void GenerateFileFromXml(XmlNode node, string path)
      {
         var newPath = Path.Combine(path, node.Attributes.GetNamedItem("FileName").Value);

         var base64 = node.InnerText;
         var bytes = Convert.FromBase64String(base64);

         File.WriteAllBytes(newPath, bytes);
      }
   }
}