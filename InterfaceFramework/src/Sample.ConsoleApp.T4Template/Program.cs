using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Sample.ConsoleApp.T4Template
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonContents = File.ReadAllText(@"D:\\Y\\Repository\repos\\Personal.Sample\\InterfaceFramework\\src\\Sample.ConsoleApp.T4Template\\Demo.json",Encoding.UTF8);

            var Model = JsonConvert.DeserializeObject<RequestModel>(jsonContents);
            TextTemplate1 template1 = new TextTemplate1(Model);
            
            string contents = template1.TransformText();
            string path = Directory.GetCurrentDirectory().Replace("bin\\Debug\\netcoreapp2.1", "assembly\\")+ Model.ApiName + ".cs";
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, contents.ToString(), Encoding.UTF8);
       
            


            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
