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

namespace MoveVideos
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
            try
            {
                SysLog.WriteOptDisk("开始迁移文件", AppDomain.CurrentDomain.BaseDirectory);
                ReadIniFile readIni = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\config.ini");
                string strIDs = readIni.ReadValue("Projects", "ProjectID");
                if (!string.IsNullOrEmpty(strIDs))
                {
                    string[] ids = strIDs.Split(',');
                    foreach (string id in ids)
                    {
                        if (id == "1")
                        {
                            Thread thr1 = new Thread(MoveESC);
                            thr1.Start();
                            thr_count++;
                        }
                        else if (id == "2")
                        {
                            Thread thr2 = new Thread(MoveJCX);
                            thr2.Start();
                            thr_count++;
                        }
                        else if (id == "3")
                        {
                            Thread thr3 = new Thread(MoveFXC_Video);
                            thr3.Start();
                            thr_count++;
                            Thread thr4 = new Thread(MoveFXC_Photo);
                            thr4.Start();
                            thr_count++;
                        }
                    }
                }
                timer1.Start();
            }
            catch (Exception ex)
            {
                SysLog.WriteLog(ex, AppDomain.CurrentDomain.BaseDirectory);
                closeForm();
            }
        }
        /// <summary>
        /// 退出程序函数
        /// </summary>
        private void closeForm()
        {
            this.Close();
            Environment.Exit(0);
        }
        /// <summary>
        /// 二手车项目迁移录像文件函数
        /// </summary>
        private void MoveESC()
        {
            try
            {
                SysLog.WriteOptDisk("开始迁移二手车录像文件", AppDomain.CurrentDomain.BaseDirectory);
                string strSourcePath_esc = "";
                string strTargetPath_esc = "";
                string strTimeLength_esc = "0";
                ReadIniFile readProject1 = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project1.ini");
                strSourcePath_esc = readProject1.ReadValue("SourcePath", "Path").Trim(); // 如 D:\Video\
                strTargetPath_esc = readProject1.ReadValue("TargetPath", "Path").Trim();
                strTimeLength_esc = readProject1.ReadValue("TimeLength", "Length").Trim();
                if (!string.IsNullOrEmpty(strSourcePath_esc) && !string.IsNullOrEmpty(strTargetPath_esc) && !string.IsNullOrEmpty(strTimeLength_esc))
                {
                    if (strSourcePath_esc.Substring(strSourcePath_esc.Length - 1, 1) != "\\")
                    {
                        strSourcePath_esc += "\\";
                    }
                    if (strTargetPath_esc.Substring(strTargetPath_esc.Length - 1, 1) != "\\")
                    {
                        strTargetPath_esc += "\\";
                    }
                    DateTime dt = DateTime.Now.AddDays(-Convert.ToInt32(strTimeLength_esc));
                    //二手车源路径存放规则 E:/Video/benwang||changsanjiao||tongyuan||.../年/月/日/所有车牌/....
                    if (Directory.Exists(strSourcePath_esc))
                    {
                        DirectoryInfo dir = new DirectoryInfo(strSourcePath_esc);
                        if (dir.Exists)
                        {
                            DirectoryInfo[] folders = dir.GetDirectories(); //返回当前目录的子目录
                            if (folders != null && folders.Length > 0)
                            {
                                foreach (DirectoryInfo folder in folders)
                                {
                                    string source = strSourcePath_esc + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    string target = strTargetPath_esc + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    if (Directory.Exists(source))
                                    {
                                        int result = Framework.Utility.WINAPI.sdnFileOperation.Move(source, target);
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
        /// 检测线项目迁移录像文件函数
        /// </summary>
        private void MoveJCX()
        {
            try
            {
                SysLog.WriteOptDisk("开始迁移检测线录像文件", AppDomain.CurrentDomain.BaseDirectory);
                string strSourcePath_jcx = "";
                string strTargetPath_jcx = "";
                string strTimeLength_jcx = "0";
                ReadIniFile readProject2 = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project2.ini");
                strSourcePath_jcx = readProject2.ReadValue("SourcePath", "Path").Trim();
                strTargetPath_jcx = readProject2.ReadValue("TargetPath", "Path").Trim();
                strTimeLength_jcx = readProject2.ReadValue("TimeLength", "Length").Trim();
                if (!string.IsNullOrEmpty(strSourcePath_jcx) && !string.IsNullOrEmpty(strTargetPath_jcx) && !string.IsNullOrEmpty(strTimeLength_jcx))
                {
                    //检测线录像存放路径规则 如 E:\Video\anjun||fengshun||...\年\月\日\1||2||...\
                    if (strSourcePath_jcx.Substring(strSourcePath_jcx.Length - 1, 1) != "\\")
                    {
                        strSourcePath_jcx += "\\";
                    }
                    if (strTargetPath_jcx.Substring(strTargetPath_jcx.Length - 1, 1) != "\\")
                    {
                        strTargetPath_jcx += "\\";
                    }
                    DateTime dt = DateTime.Now.AddDays(-Convert.ToInt32(strTimeLength_jcx));
                    if (Directory.Exists(strSourcePath_jcx))
                    {
                        DirectoryInfo dir = new DirectoryInfo(strSourcePath_jcx);
                        if (dir.Exists)
                        {
                            DirectoryInfo[] folders = dir.GetDirectories(); //返回当前目录的子目录
                            if (folders != null && folders.Length > 0)
                            {
                                foreach (DirectoryInfo folder in folders)
                                {
                                    string source = strSourcePath_jcx + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    string target = strTargetPath_jcx + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    if (Directory.Exists(source))
                                    {
                                        int result = Framework.Utility.WINAPI.sdnFileOperation.Move(source, target);
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
        /// 非现场项目录像文件迁移函数
        /// </summary>
        private void MoveFXC_Video()
        {
            try
            {
                SysLog.WriteOptDisk("开始迁移非现场录像文件", AppDomain.CurrentDomain.BaseDirectory);
                string strVideoSourcePath_fxc = "";
                string strVideoTargetPath_fxc = "";
                string strVideoTimeLength_fxc = "0";
                ReadIniFile readProject3 = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project3.ini");
                strVideoSourcePath_fxc = readProject3.ReadValue("VideoSourcePath", "Path").Trim();
                strVideoTargetPath_fxc = readProject3.ReadValue("VideoTargetPath", "Path").Trim();
                strVideoTimeLength_fxc = readProject3.ReadValue("VideoTimeLength", "Length").Trim();
                //非现场录像迁移
                //录像存放规则 H:\video\VIDEOS\年\月\日\....
                if (!string.IsNullOrEmpty(strVideoSourcePath_fxc) && !string.IsNullOrEmpty(strVideoTargetPath_fxc) && !string.IsNullOrEmpty(strVideoTimeLength_fxc))
                {
                    if (strVideoSourcePath_fxc.Substring(strVideoSourcePath_fxc.Length - 1, 1) != "\\")
                    {
                        strVideoSourcePath_fxc += "\\";
                    }
                    if (strVideoTargetPath_fxc.Substring(strVideoTargetPath_fxc.Length - 1, 1) != "\\")
                    {
                        strVideoTargetPath_fxc += "\\";
                    }
                    if (Directory.Exists(strVideoSourcePath_fxc))
                    {
                        DirectoryInfo dir = new DirectoryInfo(strVideoSourcePath_fxc);
                        if (dir.Exists)
                        {
                            DirectoryInfo[] folders = dir.GetDirectories();
                            if (folders != null && folders.Length > 0)
                            {
                                DateTime dt = DateTime.Now.AddDays(-Convert.ToInt32(strVideoTimeLength_fxc));
                                foreach (DirectoryInfo folder in folders)
                                {
                                    string source = strVideoSourcePath_fxc + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    string target = strVideoTargetPath_fxc + folder + "\\" + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                                    if (Directory.Exists(source))
                                    {
                                        int count = Framework.Utility.WINAPI.sdnFileOperation.Move(source, target);
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
        /// 非现场项目图片文件迁移函数
        /// </summary>
        private void MoveFXC_Photo()
        {
            try
            {
                SysLog.WriteOptDisk("开始迁移非现场图片文件", AppDomain.CurrentDomain.BaseDirectory);
                string strPhotoSourcePath_fxc = "";
                string strPhotoTargetPath_fxc = "";
                string strPhotoTimeLength_fxc = "0";
                ReadIniFile readProject3 = new ReadIniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\Project3.ini");
                strPhotoSourcePath_fxc = readProject3.ReadValue("PhotoSourcePath", "Path").Trim();
                strPhotoTargetPath_fxc = readProject3.ReadValue("PhotoTargetPath", "Path").Trim();
                strPhotoTimeLength_fxc = readProject3.ReadValue("PhotoTimeLength", "Length").Trim();
                //非现场图片迁移
                //图片存放格式 如 H:\IISWEB\MonitorService\photos\年\月\日\....
                if (!string.IsNullOrEmpty(strPhotoSourcePath_fxc) && !string.IsNullOrEmpty(strPhotoTargetPath_fxc) && !string.IsNullOrEmpty(strPhotoTimeLength_fxc))
                {
                    if (strPhotoSourcePath_fxc.Substring(strPhotoSourcePath_fxc.Length - 1, 1) != "\\")
                    {
                        strPhotoSourcePath_fxc += "\\";
                    }
                    if (strPhotoTargetPath_fxc.Substring(strPhotoTargetPath_fxc.Length - 1, 1) != "\\")
                    {
                        strPhotoTargetPath_fxc += "\\";
                    }
                    if (Directory.Exists(strPhotoSourcePath_fxc))
                    {
                        DateTime dt = DateTime.Now.AddDays(-Convert.ToInt32(strPhotoTimeLength_fxc));
                        string source = strPhotoSourcePath_fxc + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                        string target = strPhotoTargetPath_fxc + dt.Year.ToString("0000") + "\\" + dt.Month.ToString("00") + "\\" + dt.Day.ToString("00");
                        if (Directory.Exists(source))
                        {
                            int count = Framework.Utility.WINAPI.sdnFileOperation.Move(source, target);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thr_count == 0)
            {
                SysLog.WriteOptDisk("文件迁移结束", AppDomain.CurrentDomain.BaseDirectory);
                closeForm();
            }
        }
    }
}
