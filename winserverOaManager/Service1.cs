using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        // protected override void OnStart(string[] args)  //
        public void OnStart()
        {
            Thread th = new Thread(sdnStartServer);
            th.Start();
            //Thread threadForm = null;
            //threadForm = new Thread(FormShow);
            //threadForm.Start("sd");
            //sdnStartServer();
        }

        private void sdnStartServer()
        {
            // Thread.Sleep(10000);
            Thread threadForm = null;
            int sdnId = 32;//默认id
            ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "config.ini");
            string strId = readIni.ReadValue("userid", "uid");
            if (!string.IsNullOrEmpty(strId))
            {
                sdnId = Convert.ToInt32(strId);
            }

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
                        threadForm = new Thread(FormShow);
                        threadForm.Start(strRes);

                        //  FormShow(strRes);
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

        #region 显示窗口
        [DllImport("user32.dll")]
        static extern int GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetProcessWindowStation();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThreadId();
        [DllImport("user32.dll")]
        static extern IntPtr GetThreadDesktop(IntPtr dwThread);
        [DllImport("user32.dll")]
        static extern IntPtr OpenWindowStation(string a, bool b, int c);
        [DllImport("user32.dll")]
        static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags,
        bool fInherit, uint dwDesiredAccess);
        [DllImport("user32.dll")]
        static extern IntPtr CloseDesktop(IntPtr p);
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern IntPtr RpcImpersonateClient(int i);

        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern IntPtr RpcRevertToSelf();
        [DllImport("user32.dll")]
        static extern IntPtr SetThreadDesktop(IntPtr a);
        [DllImport("user32.dll")]
        static extern IntPtr SetProcessWindowStation(IntPtr a);
        [DllImport("user32.dll")]
        static extern IntPtr CloseWindowStation(IntPtr a);
        private void FormShow(object strMSG)
        {

            alter f = new alter(); //此FORM1可以带notifyIcon，可以显示在托盘里，用户可点击托盘图标进行设置
            f.strMsg = strMSG.ToString();//
            f.Show();
            GetDesktopWindow();
            IntPtr hwinstaSave = GetProcessWindowStation();
            IntPtr dwThreadId = GetCurrentThreadId();
            IntPtr hdeskSave = GetThreadDesktop(dwThreadId);
            IntPtr hwinstaUser = OpenWindowStation("WinSta0", false, 33554432);
            if (hwinstaUser == IntPtr.Zero)
            {
                RpcRevertToSelf();
                return;
            }
            SetProcessWindowStation(hwinstaUser);
            IntPtr hdeskUser = OpenDesktop("Default", 0, false, 33554432);
            RpcRevertToSelf();
            if (hdeskUser == IntPtr.Zero)
            {
                SetProcessWindowStation(hwinstaSave);
                CloseWindowStation(hwinstaUser);
                return;
            }
            SetThreadDesktop(hdeskUser);
            IntPtr dwGuiThreadId = dwThreadId;
            alter f = new alter(); //此FORM1可以带notifyIcon，可以显示在托盘里，用户可点击托盘图标进行设置
            f.strMsg = strMSG.ToString();//
            System.Windows.Forms.Application.Run(f);

            dwGuiThreadId = IntPtr.Zero;
            SetThreadDesktop(hdeskSave);
            SetProcessWindowStation(hwinstaSave);
            CloseDesktop(hdeskUser);
            CloseWindowStation(hwinstaUser);
        }

        #endregion

        protected override void OnStop()
        {
        }
    }
}
