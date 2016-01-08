using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinServerLookDog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SysLog.WriteOptDisk("监管程序开始执行", AppDomain.CurrentDomain.BaseDirectory);
                ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\config.ini");
                string projectIDs = readIni.ReadValue("Projects", "ProjectID").Trim();
                if (!string.IsNullOrEmpty(projectIDs))
                {
                    string[] ids = projectIDs.Split(',');
                    foreach (string id in ids)
                    {
                        if (id == "1")
                        {
                            // 二手车项目监管
                            SysLog.WriteOptDisk("开始监管二手车录像服务", AppDomain.CurrentDomain.BaseDirectory);
                            try
                            {
                                OperProcess pro = new OperProcess(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project1.ini");
                                pro.sdnStopExe(); //如果当前存在线程，则关闭
                                pro.sdnOpenExe(); //开启维护程序
                                Thread thr1 = new Thread(pro.ManageExe);
                                thr1.Start();
                            }
                            catch (Exception ex)
                            {
                                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
                            }
                        }
                        else if (id == "2")
                        {
                            // 检测线项目监管
                            SysLog.WriteOptDisk("开始监管检测线录像服务", AppDomain.CurrentDomain.BaseDirectory);
                            try
                            {
                                OperProcess pro = new OperProcess(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project2.ini");
                                pro.sdnStopExe(); //如果当前存在线程，则关闭
                                pro.sdnOpenExe(); //开启维护程序
                                Thread thr2 = new Thread(pro.ManageExe);
                                thr2.Start();
                            }
                            catch (Exception ex)
                            {
                                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
                            }
                        }
                        else if (id == "3")
                        {
                            // 非现场项目监管
                            SysLog.WriteOptDisk("开始监管非现场录像服务", AppDomain.CurrentDomain.BaseDirectory);
                            try
                            {
                                OperProcess pro = new OperProcess(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project3.ini");
                                pro.sdnStopExe(); //如果当前存在线程，则关闭
                                pro.sdnOpenExe(); //开启维护程序
                                Thread thr3 = new Thread(pro.ManageExe);
                                thr3.Start();
                            }
                            catch (Exception ex)
                            {
                                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysLog.WriteOptDisk("监管录像服务程序退出", AppDomain.CurrentDomain.BaseDirectory);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
