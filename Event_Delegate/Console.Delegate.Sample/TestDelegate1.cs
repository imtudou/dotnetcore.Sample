using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Delegate.Sample
{
    public class TestDelegate1
    {
        public List<Student> Students = new List<Student>
        {
            new Student { Id = 1,Name = "zs",Age = 15,Height = 169},
            new Student { Id = 2,Name = "ls",Age = 18,Height = 188},
            new Student { Id = 3,Name = "ws",Age = 20,Height = 190},
        };

        public List<Student> GetAge18()
        {
            var result = new List<Student>();
            foreach (var item in Students)
            {
                if (item.Age >= 18)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public List<Student> GetHeight180()
        {
            var result = new List<Student>();
            foreach (var item in Students)
            {
                if (item.Height >= 180)
                {
                    result.Add(item);
                }
            }
            return result;
        }

    }
}
