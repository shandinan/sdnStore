using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerFilesManage
{
    public partial class Form1 : Form
    {
        private int thr_count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\config.ini");
            string pros = readIni.ReadValue("Projects", "ProjectID");
            if (!string.IsNullOrEmpty(pros))
            {
                string[] ids = pros.Split(',');
                foreach (string id in ids)
                {
                    if (id == "1") //二手车
                    {
                        Thread thr_esc = new Thread(Manage_ESC);
                        thr_esc.Start();
                        SysLog.WriteOptDisk("开始执行二手车项目管理线程", AppDomain.CurrentDomain.BaseDirectory);
                        thr_count++;
                    }
                    else if (id == "2") //检测线
                    {
                        Thread thr_jcx = new Thread(Manage_JCX);
                        thr_jcx.Start();
                        SysLog.WriteOptDisk("开始执行检测线项目录像管理线程", AppDomain.CurrentDomain.BaseDirectory);
                        thr_count++;
                    }
                    else if (id == "3") //非现场
                    {
                        Thread thr_fxc_video = new Thread(Manage_FXC_Video);
                        thr_fxc_video.Start();
                        SysLog.WriteOptDisk("开始执行非现场项目录像管理线程", AppDomain.CurrentDomain.BaseDirectory);
                        thr_count++;
                        Thread thr_fxc_photo = new Thread(Manage_FXC_Photo);
                        thr_fxc_photo.Start();
                        SysLog.WriteOptDisk("开始执行非现场项目图片管理线程", AppDomain.CurrentDomain.BaseDirectory);
                        thr_count++;
                    }
                    else
                    {
                        SysLog.WriteOptDisk("项目ID配置有误", AppDomain.CurrentDomain.BaseDirectory);
                    }
                }
                timer1.Start();
            }
            else
            {
                SysLog.WriteOptDisk("项目ID配置不可为空", AppDomain.CurrentDomain.BaseDirectory);
                CloseForm();
            }
        }
        /// <summary>
        /// 计时器 待所有线程执行完成后 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thr_count == 0)
            {
                SysLog.WriteOptDisk("服务器文件周期性管理执行结束", AppDomain.CurrentDomain.BaseDirectory);
                CloseForm();
            }
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        private void CloseForm()
        {
            this.Close();
            Environment.Exit(0);
        }
        /// <summary>
        /// 二手车项目录像管理
        /// </summary>
        private void Manage_ESC()
        {
            try
            {
                ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project1.ini");
                string path = readIni.ReadValue("SourcePath", "Path");
                string time = readIni.ReadValue("TimeLength", "Length");
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(time))
                {
                    int iTime = 300; //默认保存的文件最短时长为10个月
                    if (Convert.ToInt32(time) > 300)
                    {
                        iTime = Convert.ToInt32(time);
                    }
                    DateTime dt = DateTime.Now.AddDays(-iTime);
                    if (Directory.Exists(path))
                    {
                        if (path.Substring(path.Length - 1, 1) != "\\")
                        {
                            path += "\\";
                        }
                        //二手车录像存放路径格式 D:\video\多个市场名\年\月\日\......
                        DirectoryInfo dir = new DirectoryInfo(path);
                        if (dir.Exists)
                        {
                            DirectoryInfo[] folders = dir.GetDirectories();
                            if (folders != null && folders.Length > 0)
                            {
                                foreach (DirectoryInfo folder in folders)
                                {
                                    string strPath = path + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    if (Directory.Exists(strPath))
                                    {
                                        Framework.Utility.WINAPI.sdnFileOperation.Delete(strPath,false);
                                        SysLog.WriteOptDisk("删除路径" + strPath + "成功", AppDomain.CurrentDomain.BaseDirectory);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
            finally
            {
                thr_count--;
            }
        }
        /// <summary>
        /// 检测线项目录像管理
        /// </summary>
        private void Manage_JCX()
        {
            try
            {
                ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project2.ini");
                string path = readIni.ReadValue("SourcePath", "Path");
                string time = readIni.ReadValue("TimeLength", "Length");
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(time))
                {
                    int iTime = 300; //默认检测线项目录像文件保存时长为10个月
                    if (Convert.ToInt32(time) > 300)
                    {
                        iTime = Convert.ToInt32(time);
                    }
                    DateTime dt=DateTime.Now.AddDays(-iTime);
                    if (Directory.Exists(path))
                    {
                        //检测线项目录像存放路径格式 D:\Video\各市场名称\年\月\日\.....
                        if (path.Substring(path.Length - 1, 1)!="\\")
                        {
                            path += "\\";
                        }
                        DirectoryInfo dir = new DirectoryInfo(path);
                        if (dir.Exists)
                        {
                            DirectoryInfo[] folders = dir.GetDirectories();
                            if (folders != null && folders.Length > 0)
                            {
                                foreach (DirectoryInfo folder in folders)
                                {
                                    string strPath = path + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    if (Directory.Exists(strPath))
                                    {
                                        Framework.Utility.WINAPI.sdnFileOperation.Delete(strPath,true);
                                        SysLog.WriteOptDisk("删除路径" + strPath + "成功", AppDomain.CurrentDomain.BaseDirectory);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
            finally
            {
                thr_count--;
            }
        }
        /// <summary>
        /// 非现场项目录像文件管理
        /// </summary>
        private void Manage_FXC_Video()
        {
            try
            {
                ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project3.ini");
                string path = readIni.ReadValue("VideoSourcePath", "Path");
                string time = readIni.ReadValue("VideoTimeLength", "Length");
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(time))
                {
                    int iTime = 300; //非现场录像文件默认保存时长为10个月
                    if (Convert.ToInt32(time) > 300)
                    {
                        iTime = Convert.ToInt32(time);
                    }
                    DateTime dt = DateTime.Now.AddDays(-iTime);
                    if (Directory.Exists(path))
                    {
                        //非现场录像文件存放路径格式 D:\videos\VIDEOS\年\月\日\......
                        if (path.Substring(path.Length - 1, 1) != "\\")
                        {
                            path += "\\";
                        }
                        string strPath = path + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                        if (Directory.Exists(strPath))
                        {
                            Framework.Utility.WINAPI.sdnFileOperation.Delete(strPath,true);
                            SysLog.WriteOptDisk("删除路径" + strPath + "成功", AppDomain.CurrentDomain.BaseDirectory);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
            finally
            {
                thr_count--;
            }
        }
        /// <summary>
        /// 非现场项目图片文件管理
        /// </summary>
        private void Manage_FXC_Photo()
        {
            try
            {
                ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project3.ini");
                string path = readIni.ReadValue("PhotoSourcePath", "Path");
                string time = readIni.ReadValue("PhotoTimeLength", "Length");
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(time))
                {
                    int iTime = 300; //非现场图片文件默认保存时长为10个月
                    if (Convert.ToInt32(time) > 300)
                    {
                        iTime = Convert.ToInt32(time);
                    }
                    DateTime dt = DateTime.Now.AddDays(-iTime);
                    if (Directory.Exists(path))
                    {
                        //非现场图片存放路径格式 D:\photos\年\月\日\...
                        if (path.Substring(path.Length - 1, 1) != "\\")
                        {
                            path += "\\";
                        }
                        string strPath = path + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                        if (Directory.Exists(strPath))
                        {
                            Framework.Utility.WINAPI.sdnFileOperation.Delete(strPath,true);
                            SysLog.WriteOptDisk("删除路径" + strPath + "成功", AppDomain.CurrentDomain.BaseDirectory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
            }
            finally
            {
                thr_count--;
            }
        }
    }
}
