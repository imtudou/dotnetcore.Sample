using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Timers;

namespace JSZS.Quartz.NET
{
    #region MyRegion
    //2019年9月19日16:12:03
    //真的是垃圾项目  服务器上连.NET 4.5都没装 本来说用的是 Quartz.NET 无奈只能改成timer了 

    #endregion


    class Program
    {
        private static readonly int seconds = Convert.ToInt32(ConfigurationSettings.AppSettings["seconds"]);

        static void Main(string[] args)
        {
            DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "定时任务启动......");
            Console.WriteLine("江苏中社读取付款指令状态定时任务启动......" + DateTime.Now);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = seconds; //执行间隔时间,单位为毫秒; 这里实际间隔为5分钟  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(GetPaymentWhitRecordStatus);
            Console.WriteLine("控制台应用程序启动成功......" + DateTime.Now);
            DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "定时任务启动完成......");

            Console.ReadKey();
        }

        private static void GetPaymentWhitRecordStatus(object source, ElapsedEventArgs e)
        {
            DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "------------执行方法(GetPaymentWhitRecordStatus)开始------------");

            string Available = $"{ConfigurationSettings.AppSettings["Available"].ToString()}";
            string Delegated = $"{ConfigurationSettings.AppSettings["Delegated"].ToString()}";
            string Accepted = $"{ConfigurationSettings.AppSettings["Accepted"].ToString()}";
            string Success = $"{ConfigurationSettings.AppSettings["Success"].ToString()}";
            string Failed = $"{ConfigurationSettings.AppSettings["Failed"].ToString()}";
            string Returnticket = $"{ConfigurationSettings.AppSettings["Returnticket"].ToString()}";
            string Partial_Success = $"{ConfigurationSettings.AppSettings["Partial Success"].ToString()}";
            string Expired = $"{ConfigurationSettings.AppSettings["crons"].ToString()}";


            string updateSql = "";
            try
            {
                string sql = @"SELECT DISTINCT REPLACE(ERP_PAYMENT_ID,substring(ERP_PAYMENT_ID,charindex('_',ERP_PAYMENT_ID),(LEN(ERP_PAYMENT_ID)-charindex('_',ERP_PAYMENT_ID))+1),'') as  ERP_PAYMENT_ID
                                ,RECORD_STATUS,CBS_COMMENT  FROM  AUTHORIZATION_TO_PAYMENT  WHERE RECORD_STATUS NOT IN ('Available') 
                                GROUP BY  ERP_PAYMENT_ID,RECORD_STATUS,CBS_COMMENT    ORDER BY ERP_PAYMENT_ID ASC";
                DataTable dt = clsLibrarySQLHelper.SQLHelper_Erp.GetDataSet(sql).Tables[0];
                DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "sql：" + sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + $"{dt.Rows[0]["CBS_COMMENT"].ToString()}" + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    #region 
                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Available")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + Available + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Accepted")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + Accepted + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Success")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + Success + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Failed")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + "付款失败,失败原因:" + $"{dt.Rows[0]["CBS_COMMENT"].ToString()}" + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Returnticket")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + $"{dt.Rows[0]["CBS_COMMENT"].ToString()}" + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Partial Success")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + $"{dt.Rows[0]["CBS_COMMENT"].ToString()}" + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    //if (dt.Rows[0]["RECORD_STATUS"].ToString() == "Expired")
                    //    updateSql += "UPDATE  Fa_PayMoneyMain SET  RECORD_STATUS = '" + $"{dt.Rows[0]["CBS_COMMENT"].ToString()}" + @"' WHERE BillId = " + Convert.ToInt32(dt.Rows[0]["ERP_PAYMENT_ID"]) + " ";

                    #endregion
                }
                DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "updateSql：" + updateSql);
                clsLibrarySQLHelper.SQLHelper_Erp.ExecuteSQl(updateSql);

            }
            catch (Exception ex)
            {
                DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "异常：" + ex.Message);
                
            }
            DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "------------执行方法(GetPaymentWhitRecordStatus)结束------------");





        }
    }
}
