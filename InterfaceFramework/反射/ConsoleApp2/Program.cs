using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ConsoleApp2
{
    //1.反射 动态创建类的实例,可以通过类名，成员的名字来进行对象的实例化，操作类的成员
    //2.Type类,  FieldInfo类（获取字段）,  MemberInfo类（获取方法）
    //3.通过 Activator 类在运行时动态构造对象
    //4.反射破单例（构造函数私有）
    //5.BindingFlags
    class Program
    {
        static void Main(string[] args)
        {
            Type t = Type.GetType("ConsoleApp2.Person");
            //实例化对象，默认实例化具有Public权限无参的构造方法来实例化
            object obj = Activator.CreateInstance(t);

            #region 获取实例
            //实例化对象，匹配任何权限的无参构造方法实例化
            //object obj = Activator.CreateInstance(t,true);

            //实例化对象，实例化具有Public权限有参的构造方法来实例化
            //object obj = Activator.CreateInstance(t,new object[] { 1, "1", 1.11 });

            //实例化对象，匹配任何权限的无参构造方法实例化
            //object obj = Activator.CreateInstance(t, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new object[] { 1, "1", 1.11 },null);

            #endregion

            //同过反射 访问类中的字段
            #region 获取字段
            //1.访问Public 权限的非静态字段
            System.Reflection.FieldInfo fieldA = t.GetField("A");
            fieldA.SetValue(obj, 1);        //给obj的字段A 赋值为1
            object a = fieldA.GetValue(obj);//获取对象obj 的字段 A的值

            //2.访问非Public 权限的非静态字段
            System.Reflection.FieldInfo fieldB = t.GetField("B", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldB.SetValue(obj, 2);
            object b = fieldB.GetValue(obj);

            //3.访问Public 权限的静态字段
            System.Reflection.FieldInfo fieldC = t.GetField("C", BindingFlags.Public | BindingFlags.Static);
            fieldC.SetValue(obj, 3);// fieldC.SetValue(null, 3) 如果是一个静态成员 访问的主体传null 也可以  
            object c = fieldC.GetValue(obj);

            //4.访问非Public 权限的静态字段
            System.Reflection.FieldInfo fieldD = t.GetField("A", BindingFlags.Public | BindingFlags.Static);
            fieldC.SetValue(obj, 4);
            object d = fieldA.GetValue(obj);
            #endregion


            MemberInfo member = t.GetMethod("MethodA", BindingFlags.Public | BindingFlags.Instance);

            Console.ReadKey();
        }
    }


    public class Person
    {
        #region 构造方法
        public Person()
        {
            Console.WriteLine("Hello World!");
        }

        private Person(int a, string b, double d)
        {
            Console.WriteLine("Hello World!");
        }
        #endregion


        #region 字段
        public int A;
        private int B;
        public static int C;
        private static int D;
        #endregion


        #region 方法
        public void MethodA() { }
        private void MethodB() { }
        public static void MethodC() { }
        private static void MethodD() { }
        public int Method(int a, double b)
        {
            return a;
        }
        public double Method(double a, int b)
        {
            return a;
        } 
        #endregion

    }

}
