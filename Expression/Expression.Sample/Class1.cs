using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Expression.Sample
{
    public class Class1
    { 
        public TSource Get<TSource>(Expression<Func<TSource, bool>> expression) where TSource : class
        {
            //expression.Update
            return null;
        }

        public UserInfo GetUserInfo()
        {
            return this.Get<UserInfo>(s => s.Age == 19);
        }

    }

    public class TestClass
    {
        public void Get()
        {
            var (p1,p2) = new TestClass().GetP1andP2(string.Empty);
            if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
            {
                // dosomething
            }
        }

        private (string p1, string p2) GetP1andP2(string ids)
        {
            var p1s = "p1";
            var p2s = "p2";
            return (p1s, p2s);
        }


        private static void Main()
        {
            var t = (Sum: 4.5, Count: 3);
            Console.WriteLine($"Sum of {t.Count} elements is {t.Sum}.");

            (double Sum, int Count) d = (4.5, 3);
            Console.WriteLine($"Sum of {d.Count} elements is {d.Sum}.");
        }

        

    }

    public class UserInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
