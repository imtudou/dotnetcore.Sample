using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new Product
            {
                Id = "111",
                Name = "Test1",
                Detail = new List<ProductDetail>
                {
                    new ProductDetail{Id="111" ,DtlId="1",Number=12.3568M,Price=5.689M,Amount=70.2978352M},
                    new ProductDetail{Id="111",DtlId="2",Number=12.35M,Price=5.689M,Amount=70.2978352M},
                    new ProductDetail{Id="111",DtlId="3",Number=12.358M,Price=5.689M,Amount=70.304662M},
                }
            };
            FromatDitits<Product>(model);
            Console.WriteLine("----------------------------");
            Console.ReadKey();
        }

        static void FromatDitits<T>(T model)
        {
            var newType = model.GetType();
            foreach (var item in newType.GetRuntimeProperties())
            {
                var type = item.PropertyType.Name;
                var IsGenericType = item.PropertyType.IsGenericType;
                var list = item.PropertyType.GetInterface("IEnumerable", false);
                Console.WriteLine($"属性名称：{item.Name}，类型：{type}，值：{item.GetValue(model)}");
                if (IsGenericType && list != null)
                {
                    var listVal = item.GetValue(model) as IEnumerable<object>;
                    if (listVal == null) continue;
                    foreach (var aa in listVal)
                    {
                        var dtype = aa.GetType();
                        foreach (var bb in dtype.GetProperties())
                        {
                            var dtlName = bb.Name.ToLower();
                            var dtlType = bb.PropertyType.Name;
                            var oldValue = bb.GetValue(aa);
                            if (dtlType == typeof(decimal).Name)
                            {
                                int dit = 4;
                                if (dtlName.Contains("price") || dtlName.Contains("amount"))
                                    dit = 2;
                                bb.SetValue(aa, Math.Round(Convert.ToDecimal(oldValue), dit, MidpointRounding.AwayFromZero));
                            }
                            Console.WriteLine($"子级属性名称：{dtlName}，类型：{dtlType}，值：{oldValue}");
                        }
                    }
                }
            }
        }
    }
    class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ProductDetail> Detail { get; set; }
        public List<ProductComment> Comment { get; set; }
    }
    class ProductDetail
    {
        public string DtlId { get; set; }
        public string Id { get; set; }
        public decimal Number { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
    class ProductComment
    {
        public string DtlId { get; set; }
        public string Id { get; set; }
        public string Comment { get; set; }
    }
}
