using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CodeGenerator
{
   public class JsonGenerator
   {
      private readonly string _fileName;
      private string _jsonObject;

      public JsonGenerator(string fileName)
      {
         _fileName = fileName;
      }

      public void Generate(string solutionPath, string jsonPath)
      {
         if (File.Exists(jsonPath)) return;
         GetDirectoryAsJson(solutionPath);
         Save(jsonPath);
      }

      private void GetDirectoryAsJson(string path)
      {
         _jsonObject = GetDirectoryAsJObject(new DirectoryInfo(path)).ToString();
      }

      private JObject GetDirectoryAsJObject(DirectoryInfo directory)
      {
         var obj = new JObject();

         // Directories
         foreach (var d in directory.EnumerateDirectories())
         {
            obj.Add(d.Name, GetDirectoryAsJObject(d));
         }

         // Files
         foreach (var f in directory.GetFiles())
         {
            var bytes = File.ReadAllBytes(f.FullName);
            var base64 = Convert.ToBase64String(bytes);
            obj.Add(f.Name, JValue.CreateString(base64));
         }

         return obj;
      }



      public void Save(string path)
      {
         File.WriteAllText(Path.Combine(path, _fileName), _jsonObject);
      }
   }
}
