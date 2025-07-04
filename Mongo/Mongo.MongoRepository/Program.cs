using Mongo.MongoRepository.Entities;

using MongoRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace Mongo.MongoRepository
{
    class Program
    {
        static void Main(string[] args)
        {
            var masterDataManage_MongoServerSeeting = ConfigurationManager.AppSettings["MasterDataManage_MongoServerSettings"];
            var testDB_MongoServerSettings = ConfigurationManager.AppSettings["TestDB_MongoServerSettings"];
            MongoRepository<TableInfosEntity> tableInfosEntities = new MongoRepository<TableInfosEntity>(masterDataManage_MongoServerSeeting);
            var tableInfo = tableInfosEntities.GetById("5e69a901f4e4f40bc4f8e6e3");
            Console.WriteLine(JsonConvert.SerializeObject(tableInfo));


            Console.ReadKey();
        }
    }
}
