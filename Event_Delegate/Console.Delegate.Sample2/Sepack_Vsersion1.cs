using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Delegate.Sample2
{
    public class Sepack_Vsersion1
    {
        public void SaySomething(LanguageEnum languageEnum, string name)
        {
            switch (languageEnum)
            {
                case LanguageEnum.Chinese:
                    Sepack_Say.SayChinese(name);
                    break;
                case LanguageEnum.English:
                    Sepack_Say.SayEnglish(name);
                    break;
                case LanguageEnum.Japanese:
                    break;
                case LanguageEnum.Korean:
                    break;
                default:
                    break;
            }
        }

        public static string SaySomething(LanguageEnum languageEnum) => languageEnum switch
        {
            LanguageEnum.Chinese => Do(languageEnum),
            LanguageEnum.English => Do(languageEnum),
            LanguageEnum.Japanese => Do(languageEnum),
            LanguageEnum.Korean => Do(languageEnum),
            _ => throw new NotImplementedException(),
        };

        public static string SaySomethings(LanguageEnum languageEnum, string name) => languageEnum switch
        {
            LanguageEnum.Chinese => Sepack_Say.SayChinese_str(name),
            LanguageEnum.English => Sepack_Say.SayEnglish_str(name),
            LanguageEnum.Japanese => Sepack_Say.SayJapanese_str(name),
            _ => throw new ArgumentException("error"),
        };

        private static string Do(LanguageEnum languageEnum)
        {
            return languageEnum.ToString();
        }



    }
}
