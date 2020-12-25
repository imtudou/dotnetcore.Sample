using System;
using System.Linq.Expressions;

namespace Expression.Sample
{
    public class Class1
    {
        public UserInfo Get(Expression<Func<UserInfo, UserInfo>> expression)
        {
            //expression.Update
            return null;
        
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
