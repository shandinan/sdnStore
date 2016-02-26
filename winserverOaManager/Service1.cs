using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace winserverOaManager
{
    public partial class OaService : ServiceBase
    {
        public OaService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 服务开始的时候
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

            int sdnId = 32;//默认id
            while (true)
            {
                try
                {

                    DataSet ds = GetDataLoop(sdnId);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string strRes = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            strRes += dr["Subject"] + "\r\n";
                        }
                        alter sdnShow = new alter(strRes);
                        sdnShow.Show();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                Thread.Sleep(60000); //60秒执行一次循环
            }

        }

        /// <summary>
        /// 从数据库中循环得到数据
        /// </summary>
        /// <param name="uid"></param>
        private DataSet GetDataLoop(int uid)
        {

            try
            {
                //  SqlConnection conn = new SqlConnection("server=192.1.6.33;database=sdnoa;uid=sa;pwd=Sdxk0419");
                string strSql = string.Format("select a.Subject from dbo.Mails a where a.ReceiverId={0} and a.isRead =0", uid);

                return Query(strSql);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        private DataSet Query(string SQLString)
        {

            using (SqlConnection connection = new SqlConnection("server=192.1.6.33;uid=sa;pwd=Sdxk0419;database=sdnoa;"))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }

        }

        protected override void OnStop()
        {
        }
    }
}
