using NetCore.IntefaceFramework.WebApi.CommonService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.IntefaceFramework.WebApi.T4Template.Mangers
{
    public partial class T4Template
    {
        /// <summary>
        /// 接口元数据
        /// </summary>
        public RequestModel MetaData { get; set; }

        /// <summary>
        /// 实体类
        /// </summary>
        public string Model { get; set; }
         
    }
}
