using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Framework.Data
{
    public class DbHelper
    {
        #region 成员定义
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { set; get; }
        //private DataBaseType dataBaseType;//数据库类型
        /// <summary>
        /// 数据库Provider
        /// </summary>
        public DbProviderFactory dbProviderFactory { set; get; }//数据库Provider
        public DbConnection dbConnection { set; get; }//连接对象
        public DbTransaction dbTransaction { set; get; }//事务对象
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造
        /// </summary>
        public DbHelper()
        {
        }

        /// <summary>
        /// 有参构造，实例化连接字符串
        /// </summary>
        /// <param name="provider">DbProvider</param>
        /// <param name="connectionString">连接字符串</param>
        public DbHelper(DbProviderFactory provider, string connectionString)
        {
            this.dbProviderFactory = provider;
            this.ConnectionString = connectionString;
        }
        #endregion

        #region 获取DataSet&DataTable
        /// <summary>
        /// 执行SQL语句并返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            try
            {
                using (var cmd = CreateCommand(sql))
                using (var adapter = dbProviderFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.SelectCommand.CommandTimeout = 0;
                    var dt = new DataSet();
                    adapter.Fill(dt);
                    return dt;//返回结果集
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbTransaction == null) Dispose();
            }
        }
        /// <summary>
        /// 执行SQL语句并返回DataDable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return GetDataSet(sql).Tables[0];
        }
        #endregion

        #region ExecuteNonQuery //执行SQL，并返回影响的行数
        /// <summary>
        /// 执行SQL，并返回影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, CommandType.Text, null);
        }
        /// <summary>
        ///  执行SQL，并返回影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, CommandType cmdType, params DbParameter[] dbParameters)
        {
            try
            {
                using (var cmd = CreateCommand(sql, cmdType))
                {

                    if (dbParameters != null)
                        foreach (DbParameter dbParameter in dbParameters)
                        {
                            cmd.Parameters.Add(dbParameter);
                        }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbTransaction == null) Dispose();
            }
        }
        #endregion

        #region ExecuteScalar //执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string sql)
        {
            try
            {
                using (var cmd = CreateCommand(sql))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbTransaction == null) Dispose();
            }
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string sql, CommandType cmdType, params DbParameter[] dbParameters)
        {
            try
            {
                using (var cmd = CreateCommand(sql, cmdType))
                {
                    if (dbParameters != null)
                        foreach (DbParameter dbParameter in dbParameters)
                        {
                            cmd.Parameters.Add(dbParameter);
                        }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbTransaction == null) Dispose();
            }
        }
        #endregion

        #region ExecuteReader(GetList) //执行SQL，并返回集合
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string sql)
        {
            try
            {
                using (var cmd = CreateCommand(sql))
                using (var reader = cmd.ExecuteReader())
                {
                    //IDataReader reader = cmd.ExecuteReader();
                    //实例化一个List<>泛型集合    
                    List<T> dataList = new List<T>();
                    while (reader.Read())
                    {
                        T RowInstance = Activator.CreateInstance<T>();//动态创建数据实体对象    
                                                                      //通过反射取得对象所有的Property    
                        if (RowInstance is Dictionary<string, object>)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                (RowInstance as Dictionary<string, object>).Add(reader.GetName(i), reader.GetValue(i));
                            }
                        }
                        else
                        {
                            foreach (PropertyInfo Property in typeof(T).GetProperties())
                            {
                                try
                                {
                                    //取得当前数据库字段的顺序    
                                    int Ordinal = reader.GetOrdinal(Property.Name);
                                    if (reader.GetValue(Ordinal) != DBNull.Value)
                                    {
                                        //将DataReader读取出来的数据填充到对象实体的属性里    
                                        Property.SetValue(RowInstance, Convert.ChangeType(reader.GetValue(Ordinal), Property.PropertyType), null);
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }
                        dataList.Add(RowInstance);
                    }
                    return dataList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbTransaction == null) Dispose();
            }
        }
        #endregion

        #region 基本函数(命令)
        private DbCommand CreateCommand(string cmdText, CommandType cmdType = CommandType.Text)
        {
            //创建数据库连接对象
            if (dbConnection == null || dbConnection.State != ConnectionState.Open)
            {
                dbConnection = dbProviderFactory.CreateConnection();
                dbConnection.ConnectionString = ConnectionString;
                dbConnection.Open();//打开数据库连接池
            }
            //创建Command命令
            var cmd = dbConnection.CreateCommand();
            cmd.Connection = dbConnection;
            cmd.CommandType = cmdType;
            if (!string.IsNullOrEmpty(cmdText))
                cmd.CommandText = cmdText;
            if (dbTransaction != null) cmd.Transaction = dbTransaction;
            cmd.CommandTimeout = 0;
            return cmd;
        }
        private DataSet Execute(DbCommand cmd)
        {
            try
            {
                using (var adapter = dbProviderFactory.CreateDataAdapter())//创建适配器
                {
                    adapter.SelectCommand = cmd;
                    adapter.SelectCommand.CommandTimeout = 0;
                    var dt = new DataSet();
                    adapter.Fill(dt);
                    return dt;//返回结果集
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (dbTransaction == null) Dispose();
            }
        }
        #endregion

        #region 开启事务
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public void BeginTransation()
        {
            //创建数据库连接对象
            if (dbConnection == null || dbConnection.State != ConnectionState.Open)
            {
                dbConnection = dbProviderFactory.CreateConnection();
                dbConnection.ConnectionString = ConnectionString;
                dbConnection.Open();//打开数据库连接池
            }
            dbTransaction = dbConnection.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = null;
                Dispose();
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;
                Dispose();
            }
        }
        #endregion

        #region 实现接口IDisposable
        /// <释放资源接口>
        /// 实现接口IDisposable
        /// </释放资源接口>
        public void Dispose()
        {
            if (dbConnection != null)
            {
                if (dbConnection.State == ConnectionState.Open)//判断数据库连接池是否打开
                {
                    dbConnection.Close();
                }
                dbConnection.Dispose();//释放连接池资源
                GC.SuppressFinalize(this);//垃圾回收
            }
        }
        #endregion

        #region 析构函数
        /// <summary>
        /// 析构函数
        /// </summary>
        ~DbHelper()
        {
            Dispose();
        }
        #endregion
    }
}
