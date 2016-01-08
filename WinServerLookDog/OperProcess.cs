/**
 * 操作第三方进程（程序）类
 * 时间：2015年9月10日09:14:12
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace WinServerLookDog
{
    public class OperProcess
    {
        #region 定义变量
        public Process p = null; //控制第三方进程的进程名称
        string proName = "";//要维护的第三方程序的进程名称
        int checkTimes = 0;//维护次数
        string IniName = "";
        #endregion

        #region 析构函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public OperProcess(string iniName)
        {
            IniName = iniName;
            proName = GetIniValue("ProcessName", "Name");
            checkTimes = string.IsNullOrEmpty(GetIniValue("CheckNums", "Times").Trim()) ? 0 : Convert.ToInt32(GetIniValue("CheckNums", "Times").Trim());//从配置项读取检测次数
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~OperProcess()
        {

        }

        #endregion

        #region 公共函数--读取配置文件信息

        /// <summary>
        /// 通过相应的键值对获取配置文件中的值
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private string GetIniValue(string strKey, string strValue)
        {
            if (!string.IsNullOrEmpty(strKey) && !string.IsNullOrEmpty(strValue))
            {
                ReadIniFile readini = new ReadIniFile(IniName);
                return readini.ReadValue(strKey, strValue);
            }
            else
            {
                SysLog.WriteOptDisk("配置文件键值不允许为空!", AppDomain.CurrentDomain.BaseDirectory);
                return null;
            }
        }
        #endregion

        #region 通过Process 控制第三方程序

        /// <summary>
        /// 通过Process打开进程
        /// </summary>
        public void sdnOpenExe()
        {
            try
            {
                p = new Process(); //建立外部调用线程
                string strPath = GetIniValue("ExePath", "Path"); //通过INI配置文件得到相应的程序地址
                if (string.IsNullOrEmpty(strPath))
                    return;
                p.StartInfo.FileName = strPath; //要调用外部程序的绝对路径
                p.StartInfo.UseShellExecute = false; //不使用操作系统外壳程序启动线程
                p.StartInfo.RedirectStandardError = true; //把外部程序错误输出写到StandardError流中
                p.StartInfo.CreateNoWindow = true; //不创建进程窗口

                p.ErrorDataReceived += new DataReceivedEventHandler(Output); //外部程序（这里是FFMPEG）输出流时产生的事件，这里是把流的处理过程转移到下面的方法中
                p.Start(); //启动线程
                SysLog.WriteOptDisk(proName + "重启成功", AppDomain.CurrentDomain.BaseDirectory);
            }
            catch(Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
        }
        /// <summary>
        /// 程序执行信息输出流操作方法
        /// </summary>
        /// <param name="sendProcess"></param>
        /// <param name="output"></param>
        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (!String.IsNullOrEmpty(output.Data))
            {
                //处理方法...
            }
        }

        /// <summary>
        /// 关闭EXE
        /// </summary>
        public void sdnStopExe()
        {
            try
            {
                if (p != null)
                {
                    bool blExit = p.HasExited;
                    if (!blExit)
                    {
                        p.Kill();
                        p.Close(); //关闭进程
                        p.Dispose(); //释放资源
                    }
                }
                KillProcess(); //杀死录像服务进程
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="processName">进程名称</param>
        private void KillProcess()
        {
            try
            {
                if (string.IsNullOrEmpty(proName))
                {
                    SysLog.WriteOptDisk("要维护的第三方程序，程序名称不能为空", AppDomain.CurrentDomain.BaseDirectory);
                    return;
                }
                Process[] procs = Process.GetProcessesByName(proName);
                if (procs != null && procs.Count() > 0)
                {
                    foreach (Process pro in procs)
                    {
                        if (!pro.HasExited)
                        {
                            pro.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
        }
        /// <summary>
        /// 判断EXE是否已打开
        /// </summary>
        /// <param name="processName"></param>
        private bool IsOpenEXE(string processName)
        {
            bool blFlag = false;
            try
            {
                Process[] procs = Process.GetProcesses();
                foreach (Process item in procs)
                {
                    if (item.ProcessName == processName)
                    {
                        blFlag = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
            return blFlag;
        }
        #endregion

        #region 控制维护---核心函数

        public void ManageExe()
        {
            DateTime dtNow;
            DateTime[] arrCheckTime = new DateTime[checkTimes];
            int[] arrIsCheck = new int[checkTimes];
            int isDo = 0;
            while (true)
            {
                dtNow = DateTime.Now;//当前时间
                try
                {
                    for (int i = 1; i <= checkTimes; i++)
                    {
                        string strTemp = GetIniValue("CheckTime", "Time" + i);
                        if (string.IsNullOrEmpty(strTemp))
                        {
                            SysLog.WriteOptDisk("time" + i + "为空", AppDomain.CurrentDomain.BaseDirectory);
                        }
                        else
                        {
                            arrCheckTime[i - 1] = Convert.ToDateTime(strTemp);
                        }
                    }

                    if (dtNow.Hour < 7 || dtNow.Hour > 19) //如果早上7点之前 晚上 7点之后
                    {
                        if (isDo == 0)
                        {
                            for (int i = 0; i < checkTimes; i++)
                            {
                                arrIsCheck[i] = 0;
                                arrCheckTime[i] = arrCheckTime[i].AddDays(1);
                            }
                            isDo = 1;
                        }
                        Thread.Sleep(300000); //5分钟
                    }
                    else
                    {
                        //10分钟检测一次录像服务是否正在运行 不在运行的话 需重启
                        if ((DateTime.Now.Minute % 10 == 0))
                        {
                            if (!IsOpenEXE(proName))
                            {
                                try
                                {
                                    sdnStopExe();
                                    Thread.Sleep(2000);
                                    sdnOpenExe(); //重新开启服务
                                }
                                catch { }
                            }
                        }
                        if (arrIsCheck.Any(p => p != 0))
                        {
                            Thread.Sleep(600000); //10分钟
                        }
                        isDo = 0; //空闲时间重置执行标志
                        for (int i = 0; i < checkTimes; i++)
                        {
                            if (arrIsCheck[i] == 0)
                            {
                                arrIsCheck[i] = ReOpenEXE(arrCheckTime[i]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
                    Thread.Sleep(2000);
                    continue;
                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 根据使用标志和维护时间
        /// </summary>
        /// <param name="iFlag"></param>
        /// <param name="dt"></param>
        private int ReOpenEXE(DateTime dt)
        {

            if (DateTime.Now >= dt && DateTime.Now < dt.AddMinutes(10))  //2016-01-08 原先3分钟改成10分钟
            {
                try
                {
                    sdnStopExe();
                    Thread.Sleep(2000);
                    sdnOpenExe();
                }
                catch (Exception ex)
                {
                    SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
                }
                return 1;
            }
            return 0;
        }
        #endregion
    }
}
