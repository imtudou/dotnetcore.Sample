using System;
using System.Linq;
using System.Collections.Generic;

namespace Console.Delegate.Sample
{
    class Program
    {
        /*
         * 案例一：
         * 在不考虑是使用List.Where(),Find()等方法的基础上实现，因为这心方法内部都是委托调用的
            1.获取年龄大于18岁的
            2.获取分数大于60的
            3.获取男生升高高于180的
            4.......

        第二步：
        考虑使用委托

         */
        public delegate List<Student> GetTestAgeDelegate1();
        public delegate List<Student> GetTestHeightDelegate1();

        public delegate bool TestDelegate(Student s);

        static void Main(string[] args)
        {
            //1.定义委托        public delegate void TestDelegate();
            //2.声明委托实例    TestDeleegate td = new TestDelegate();
            //3.调用            td.Invoke();

            var test1 = new TestDelegate1();
            test1.GetAge18();
            test1.GetHeight180();

            var getTestAge = new GetTestAgeDelegate1(test1.GetAge18);
            var resultAge = getTestAge.Invoke(); //Dictionary<string,int> 
            var getTestHeight = new GetTestHeightDelegate1(test1.GetHeight180);
            var resultHeight = getTestAge.Invoke(); //Dictionary<string, decimal>

            // 委托显示调用方式1
            TestDelegate tAgedelegate, tHeightdelegate;
            // age > 18
            tAgedelegate = CheckAge;
            // height > 180
            tHeightdelegate = CheckHeight;

            var rAge = Program.GetStudentList(test1.Students, tAgedelegate);
            var rHeight = Program.GetStudentList(test1.Students, tHeightdelegate);


            //委托调用方式2
            TestDelegate  testDelegate;
            // age > 18
            testDelegate = CheckAge;//先给委托类型的变量赋值
            // height > 180
            testDelegate += CheckHeight;// 给此委托变量再绑定一个方法
            var resT = Program.GetStudentList(test1.Students, testDelegate);
        }


        public static bool CheckAge(Student s)
        {
            return s.Age > 18;
        }

        public static bool CheckHeight(Student s)
        {
            return s.Height > 180;
        }

        public static bool CheckNameIsZS(Student s)
        {
            return s.Name == "zs";
        }


        public static List<Student> GetStudentList(List<Student> students, TestDelegate test)
        {
            var result = new List<Student>();
            foreach (var item in students)
            {
                if (test(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }




    }
}

