using System;
using System.Net.Http;
using Xunit;
using System.Text.Json;
using System.Text;
using System.Net;
using System.IO;

namespace FluentValidation.Sample.XunitTest
{
    public class ValuesControllerTest
    {
        [Fact]
        public void Get_Get_Value_Success()
        {
            using (HttpClient client = new HttpClient())
            {
           
                var person = new Person 
                { 
                    Age = 1,
                    Email = "xxx@qq.com",
                    Name = "xxx",
                };
                var url = "http://localhost:5000/api/values";
                var data = new StringContent(JsonSerializer.Serialize(person),Encoding.UTF8,"application/json");
                var result = client.PostAsync(url, data).Result;
                var cc1 = person.GetHashCode();
                var cc2 = Encoding.GetEncoding("utf-8").GetBytes(JsonSerializer.Serialize(person));
                FileStream fs = new FileStream("test.txt",FileMode.OpenOrCreate,FileAccess.ReadWrite);
                fs.WriteAsync(cc2).ConfigureAwait(false);



                Assert.NotNull(result);
                Assert.IsType<HttpResponseMessage>(result);
                //Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                var content = result.Content.ReadAsStringAsync().Result;
                Assert.NotNull(content);
                Assert.IsType<Person>(JsonSerializer.Deserialize<Person>(content));

            }

        }

        [Fact]
        public void Get_Get_Value_Error()
        {
            using HttpClient client = new HttpClient();
            var person = new Person
            {
                Age = 18,
                Email = "xxxxqq.com",
                Name = "xxx",
            };
            var url = "http://localhost:8082/api/values";
            var data = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
            var result = client.PostAsync(url, data).Result;

            Assert.NotNull(result);
            Assert.IsType<HttpResponseMessage>(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.NotNull(content);
            Assert.IsType<Person>(JsonSerializer.Deserialize<Person>(content));

        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
