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
            this.Close();
        }
    }
}
