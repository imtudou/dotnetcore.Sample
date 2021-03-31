using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetCore.IntefaceFramework.WebApi.assembly
{

    public class OrderInfo
    {
        public List<OrderInfoModel> ListData = new List<OrderInfoModel>() {
                new OrderInfoModel(){ID="1",Name="HUAWEI1",Price="50001",Area="China"},
                new OrderInfoModel(){ID="2",Name="HUAWEI2",Price="50002",Area="China"},
                new OrderInfoModel(){ID="3",Name="HUAWEI3",Price="50003",Area="China"},
                new OrderInfoModel(){ID="4",Name="HUAWEI4",Price="50004",Area="China"},
                new OrderInfoModel(){ID="5",Name="HUAWEI5",Price="50005",Area="China"},
                new OrderInfoModel(){ID="6",Name="HUAWEI6",Price="50006",Area="China"},
                new OrderInfoModel(){ID="7",Name="HUAWEI7",Price="50007",Area="China"}
        };


        public string GetOrderInfo()
        {
            return JsonConvert.SerializeObject(ListData);

        }


        public string GetOrderInfoByID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                throw new ArgumentException("argument is null");

            ListData = ListData.Where(s => s.ID.Equals(ID)).ToList();

            return JsonConvert.SerializeObject(ListData);
        }


    }






    public class OrderInfoModel
    {

        public string ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Num { get; set; }
        public string Area { get; set; }

    }
}

