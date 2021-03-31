using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Delegate.Sample3
{
    public class Sepack_Say
    {
        public static void SayChinese(string name)
        {
            System.Console.WriteLine($"{name}用中文:说你好！");
        }
        public static void SayChinese(string name,string province)
        {
            System.Console.WriteLine($"{name}在"+$"{province},用中文:说你好！");
        }

        public static void SayEnglish(string name)
        {
            System.Console.WriteLine($"{name}用英语:say hello！");
        }
        public static void SayEnglish(string name, string province)
        {
            System.Console.WriteLine($"{name}在" + $"{province},用英语:say hello！");
        }

        public static void SayJapanese(string name)
        {
            System.Console.WriteLine($"{name}用日语:こんにちは！");
        }


        public static string SayChinese_str(string name)
        {
            return $"{name}用中文:说你好！";
        }
        public static string SayChinese_str(string name, string province)
        {
            return  $"{name}在" + $"{province},用中文:说你好！";
        }


        public static string SayEnglish_str(string name)
        {
            return  $"{name}用英语:say hello！";
        }

        public static string SayJapanese_str(string name)
        {
            return $"{name}用日语:こんにちは！";
        }
    }
}
