using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Unit.Sample.XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            List<UserModel> lsits = new();

            //找出aaa 这一项
            //example 1
            var list = new List<UserModel>
            { 
                new UserModel { Name = "zs",NickName = "zxs",Birthday = 10},
                new UserModel { Name = "ZS",NickName = "zxs",Birthday = 10}
            };

            for (int i = 0; i < list.Count; i++)
            {

                //example 1  old
                //if (list[i].Name.ToString() == "zs" || list[i].ToString() == "ZS")
                //{
                //    //Do Something
                //}

                ////exapmle 2  c#9.0
                //if (list[i].Name.ToString() is "zs" or "ZS")
                //{

                //}

                if (list[i].Name.ToString() is "ZS" and not null)
                {

                }
            }

            //example 2
            string item = list.Find(s => s.Name.Equals("zs")).Name;
        }

        public class UserModel
        {
            public string Name { get; set; }
            public string NickName { get; set; }
            public int Birthday { get; set; }
        }
    }
}
