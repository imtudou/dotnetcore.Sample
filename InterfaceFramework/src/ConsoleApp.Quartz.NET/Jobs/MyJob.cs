using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Quartz;
using System.Data;
using System.Configuration;

namespace ConsoleApp.Quartz.NET.Jobs
{
    public class MyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //Available，等待CBS系统读取数据；
            //Delegated，CBS系统已读取数据；
            //Accepted，CBS系统已经受理；
            //Success，付款成功；
            //Failed，所有非成功的终态。
            //Returnticket，银行退票 CBS4.6新增状态
            //Partial Success 部分成功。
            //Expired 过期状态


            string Available = $"{ConfigurationManager.AppSettings["Available"].ToString()}";
            string Delegated = $"{ConfigurationManager.AppSettings["Delegated"].ToString()}";
            string Accepted = $"{ConfigurationManager.AppSettings["Accepted"].ToString()}";
            string Success = $"{ConfigurationManager.AppSettings["Success"].ToString()}";
            string Failed = $"{ConfigurationManager.AppSettings["Failed"].ToString()}";
            string Returnticket = $"{ConfigurationManager.AppSettings["Returnticket"].ToString()}";
            string Partial_Success = $"{ConfigurationManager.AppSettings["Partial Success"].ToString()}";
            string Expired = $"{ConfigurationManager.AppSettings["crons"].ToString()}";


            string updateSql = "";
            await Task.Run(() =>
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

            });
           
   
        }
    }
}