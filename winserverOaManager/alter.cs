using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sdnControls;
using System.Threading;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace winserverOaManager
{
    public partial class alter : sdnControls.sdnSkinForm.SkinForm
    {
        public string strMsg = "sfdf"; //控件要显示的内容
        public alter()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        //public alter(string strMsg)
        //{
        //    this.strMsg = strMsg;
        //}

        private void alter_Load(object sender, EventArgs e)
        {
            int Heightone = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;         //获取屏幕的高度
            int Heighttwo = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;   //获取工作区的高度
            int screenX = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;      //获取屏幕的宽度
            int renwu = Heightone - Heighttwo;          //屏幕的高度减去工作区的高度，得到任务栏的高度，只所以获取任务栏的高度，是由于一些时候任务栏的高度不固定。避免窗体被任务栏遮挡住
            this.Top = Heightone - 237 - renwu;     //距离上边的距离＝屏幕的高度－窗体的高度－任务栏的高度
            this.Left = screenX - 389;           //距离左边的距离＝屏幕的宽度－窗体的宽度
            this.Opacity = 50;     //设置窗体的不透明度为0
            lbMsgContent.Text = strMsg;//显示内容
            Thread td = new Thread(KillMyself);
            td.Start();
        }
        private void KillMyself()
        {
            Thread.Sleep(10000);//10秒后关闭
            //  MessageBox.Show("ffffff");
            this.Close();
        }

        #region 打开浏览器

        /// <summary>
        /// 调用系统浏览器打开网页
        /// </summary>
        /// <param name="url">打开网页的链接</param>
        public static void OpenBrowserUrl(string url)
        {
            try
            {
                // 64位注册表路径
                var openKey = @"SOFTWARE\Wow6432Node\Google\Chrome";
                if (IntPtr.Size == 4)
                {
                    // 32位注册表路径
                    openKey = @"SOFTWARE\Google\Chrome";
                }
                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
                // 谷歌浏览器就用谷歌打开，没找到就用系统默认的浏览器
                // 谷歌卸载了，注册表还没有清空，程序会返回一个"系统找不到指定的文件。"的bug
                var openPath = appPath != null ? "chrome.exe" : "EXPLORER.EXE";
                Process.Start(openPath, url);
            }
            catch
            {
                // 出错调用用户默认设置的浏览器，还不行就调用IE
                OpenDefaultBrowserUrl(url);
            }
        }
        /// <summary>
        /// 用IE打开浏览器
        /// </summary>
        /// <param name="url"></param>
        public static void OpenIe(string url)
        {
            try
            {
                Process.Start("iexplore.exe", url);
            }
            catch (Exception ex)
            {
              //  LogUtil.WriteException(ex);
                // IE浏览器路径安装：C:\Program Files\Internet Explorer
                // at System.Diagnostics.process.StartWithshellExecuteEx(ProcessStartInfo startInfo)注意这个错误
                try
                {
                    if (File.Exists(@"C:\Program Files\Internet Explorer\iexplore.exe"))
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = @"C:\Program Files\Internet Explorer\iexplore.exe",
                            Arguments = url,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(processStartInfo);
                    }
                    else
                    {
                        if (File.Exists(@"C:\Program Files (x86)\Internet Explorer\iexplore.exe"))
                        {
                            ProcessStartInfo processStartInfo = new ProcessStartInfo
                            {
                                FileName = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe",
                                Arguments = url,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };
                            Process.Start(processStartInfo);
                        }
                        else
                        {
                            if (MessageBox.Show("系统未安装IE浏览器，是否下载安装？", null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                // 打开下载链接，从微软官网下载
                                OpenBrowserUrl("http://windows.microsoft.com/zh-cn/internet-explorer/download-ie");
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                   // LogUtil.WriteException(exception);
                }
            }
        }
        /// <summary>
        /// 打开系统默认浏览器（用户自己设置了默认浏览器）
        /// </summary>
        /// <param name="url"></param>
        public static void OpenDefaultBrowserUrl(string url)
        {
            try
            {
                Process.Start("EXPLORER.EXE", url);
            }
            catch
            {
                OpenIe(url);
            }
        }

        #endregion

        private void lbMsgContent_Click(object sender, EventArgs e)
        {
            OpenDefaultBrowserUrl("http://192.1.6.35:10001");
        }
    }
}
