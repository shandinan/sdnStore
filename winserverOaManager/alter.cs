using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sdnControls;

namespace winserverOaManager
{
    public partial class alter : sdnControls.sdnSkinForm.SkinForm
    {
        public alter()
        {
            InitializeComponent();
            
        }
        public alter(string strMsg)
        {
            lbMsgContent.Text = strMsg;
        }

        private void alter_Load(object sender, EventArgs e)
        {
            int Heightone = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;         //获取屏幕的高度
            int Heighttwo = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;   //获取工作区的高度
            int screenX = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;      //获取屏幕的宽度
            int renwu = Heightone - Heighttwo;          //屏幕的高度减去工作区的高度，得到任务栏的高度，只所以获取任务栏的高度，是由于一些时候任务栏的高度不固定。避免窗体被任务栏遮挡住
            this.Top = Heightone - 160 - renwu;     //距离上边的距离＝屏幕的高度－窗体的高度－任务栏的高度
            this.Left = screenX - 290;           //距离左边的距离＝屏幕的宽度－窗体的宽度
            this.Opacity = 0;     //设置窗体的不透明度为0
        }
    }
}
