using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Net.Sample
{
    class Program
    {
        static void Main(string[] args)
        {


            HealthGroupBusinessLogEnum logEnum1 = (HealthGroupBusinessLogEnum)Enum.Parse(typeof(HealthGroupBusinessLogEnum), "1101");

            var css1 = GetRandomUpperAndLowerCode(10);


            var cssss = nameof(HealthGroupBusinessLogEnum.Success);

            string guid1 = Guid.NewGuid().ToString("N");
            string guid2 = Guid.NewGuid().ToString();




            for (int n = 0; n < 10000; n++)
            {
                object Locker = new object();
                int _sn = 0;

                lock (Locker)
                {
                    if (_sn == int.MaxValue)
                    {
                        _sn = 0;
                    }

                    else
                    {
                        _sn++;
                    }

                    string _zimu = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz1234567890";//要随机的数字和字母，排斥I,,O,o
                    Random _rand = new Random(); //随机类
                    string _result = "";
                    for (int i = 0; i < 6; i++) //循环6次，生成6位数字，10位就循环10次
                    {
                        _result += _zimu[_rand.Next(59)]; //通过索引下标随机

                    }
                    Console.WriteLine(_result);
                }

                
            }
            
           



            var trimStr = "123,124,125,".TrimEnd(',');
            var data = new Dictionary<int,int>();
            for (int i = 0; i < 1000; i++)
            {
                data.Add(i, i);
            }

            int pageSize = 100;        
            int count = data.Count / pageSize;
            for (int i = 0; i < count; i++)
            {
                var mainData = data.Skip(i * pageSize).Take(pageSize).ToList();   
                for (int n = 0; n < mainData.Count; n++)
                {
                      // upload ....
                }
                Console.WriteLine();
            }


            const string Users = "HPT08";
            string content = @"{""Type"":""Esc"",""Message"":""您的帐号在另一台设备登陆"",""ChannelCode"":"""+ Users + "\"}";


            var cc = HealthGroupBusinessLogEnum.医生取消方案;
            string title = string.Empty;


            var cc1 = Enum.Parse(typeof(HealthGroupBusinessLogEnum), "患者解绑");

            HealthGroupBusinessLogEnum logEnum;
            var cc2 = Enum.TryParse("1101", out logEnum);

            Enum.GetName(typeof(HealthGroupBusinessLogEnum), title);
            Enum.GetValues(typeof(HealthGroupBusinessLogEnum));
            var c1 = Enum.GetName(typeof(HealthGroupBusinessLogEnum), "患者解绑");





            var ids = new int[5] { 1, 2, 3, 4, 5 };

            for (int i = 0; i < ids.Length; i++)
            {
                DoSomething(ids[i]);
            }
        }


        public static void DoSomething(int param)
        {
                     
        }

        public static Func<HealthOrganizationMainEntity,bool> GetFunc(string ccc)
        {
            return s => s.IsAutonomousPlace.Equals(ccc);
        }

        /// <summary>
        /// 获取数字和大小写字母的混合字符串，字母L、O排除
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetRandomUpperAndLowerCode(int count)
        {
            string code = "";
            string upperStr = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            Random rand = new Random();
            string guid = Guid.NewGuid().ToString("N").Substring(0, count - 1);
            code = upperStr[rand.Next(24)] + guid;
            return code.Trim();
        }
    }


     
    public enum HealthGroupBusinessLogEnum
    {
        /// <summary>
        /// 患者解绑
        /// </summary>
        患者解绑 = 1101,

        /// <summary>
        /// 退出方案
        /// </summary>
        退出方案,

        /// <summary>
        /// 医生取消方案
        /// </summary>
        医生取消方案,

        Success

    }

 

}


public class HealthOrganizationMainEntity
{
    /// <summary>
    /// Desc:数据库主键自增
    /// Default:
    /// Nullable:False
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public string HealthOrgizationMainID { get; set; }

    /// <summary>
    /// Desc:DE08.10.052.00/组织机构代码
    /// Default:
    /// Nullable:True
    /// </summary>
    public string OrganizationCode { get; set; }

    /// <summary>
    /// Desc:DE08.10.009.00/机构成立日期
    /// Default:
    /// Nullable:True
    /// </summary>
    public DateTime? OrganizationCreateDate { get; set; }

    /// <summary>
    /// Desc:DE08.10.010.00/机构从业人员数
    /// Default:
    /// Nullable:True
    /// </summary>
    public int? OrganizationEmployees { get; set; }

    /// <summary>
    /// Desc:DE08.10.013.00/机构名称
    /// Default:
    /// Nullable:True
    /// </summary>
    public string OrganizationName { get; set; }

    /// <summary>
    /// Desc:DE08.10.011.00/机构第二名称
    /// Default:
    /// Nullable:True
    /// </summary>
    public string OrganizationSecondName { get; set; }

    /// <summary>
    /// Desc:DE08.10.014.00/机构所在地民族自治地方标志（标识个体或机构所在地是否为民族自治地方）
    /// Default:
    /// Nullable:True
    /// </summary>
    public bool IsAutonomousPlace { get; set; }

    /// <summary>
    /// Desc:DE08.10.015.00/机构有无检验室标志
    /// Default:
    /// Nullable:True
    /// </summary>
    public bool IsHaveLaboratory { get; set; }

    /// <summary>
    /// Desc:DE08.10.017.00/稽查机构专设标志（标识机构内是否专设稽查机构）
    /// Default:
    /// Nullable:True
    /// </summary>
    public bool IsHaveCheckRoom { get; set; }

    /// <summary>
    /// Desc:DE08.10.045.00/执法考核评议标志(标识机构是否开展执法考核评议)
    /// Default:
    /// Nullable:True
    /// </summary>
    public bool IsHaveLawEnforcement { get; set; }

    /// <summary>
    /// Desc:DE08.10.046.00/职工总数
    /// Default:
    /// Nullable:True
    /// </summary>
    public int EmployeesTotal { get; set; }

    /// <summary>
    /// Desc:DE08.10.004.00/发生执法过错的下级责任单位数
    /// Default:
    /// Nullable:True
    /// </summary>
    public int UnitTotal { get; set; }

    /// <summary>
    /// Desc:DE08.10.029.00/派出(分支)机构数量(机构下设派出机构的数量)
    /// Default:
    /// Nullable:True
    /// </summary>
    public int BranchTotal { get; set; }

    /// <summary>
    /// Desc:DE08.10.001.00/机构分类管理类别代码（1.非盈利性医疗机构 2.盈利性医疗机构 9.其他）
    /// Default:
    /// Nullable:True
    /// </summary>
    public string OrganizationManageCategoryCode { get; set; }

    /// <summary>
    /// Desc:DE08.10.037.00/许可项目名称
    /// Default:
    /// Nullable:True
    /// </summary>
    public string LicenseProjectName { get; set; }

    /// <summary>
    /// Desc:DE08.10.038.00/许可证号码
    /// Default:
    /// Nullable:True
    /// </summary>
    public string LicenseNumber { get; set; }

    /// <summary>
    /// Desc:DE08.10.039.00/许可证有效期开始日期
    /// Default:
    /// Nullable:True
    /// </summary>
    public DateTime? LicenseValidStartDate { get; set; }

    /// <summary>
    /// Desc:DE08.10.051.00/租借房屋面积(m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal RentalHousAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.001.00/办公用房面积 (m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal OfficeAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.005.00/房屋建筑总面积(m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal BuildTotalAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.006.00/房屋竣工面积(m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal CompletedAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.030.00/批准基建项目建筑面积(m2)
    /// Default:
    /// Nullable:False
    /// </summary>
    public decimal ApproveAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.032.00/危房面积(m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal DangerAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.041.00/业务用房面积(m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal BusinessAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.044.00/营业面积(m2)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal BuyAndSellAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.050.00/自有房屋面积(m2)（机构自有房屋面积 ,计量单位为m2）
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal OwnHouseAcreage { get; set; }

    /// <summary>
    /// Desc:DE08.10.016.00/机构自有资金金额(万元)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal OwnMoney { get; set; }

    /// <summary>
    /// Desc:DE08.10.024.00/开办资金金额数 (万元)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal SetupMoney { get; set; }

    /// <summary>
    /// Desc:DE08.10.027.00/年度实际完成投资金额(万元)
    /// Default:
    /// Nullable:False
    /// </summary>
    public decimal ActuallyCompletedMoney { get; set; }

    /// <summary>
    /// Desc:DE08.10.028.00/年度新增固定资产金额(万元)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal FixedAssets { get; set; }

    /// <summary>
    /// Desc:DE08.10.043.00/银行贷款及其他借款金额(万元)
    /// Default:
    /// Nullable:True
    /// </summary>
    public decimal LoanMoney { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public string CreateID { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public string UpdateID { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public string TenantID { get; set; }

    /// <summary>
    /// Desc:
    /// Default:
    /// Nullable:True
    /// </summary>
    public bool IsDelete { get; set; }
}