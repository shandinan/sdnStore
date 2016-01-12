/**
 *  调用windows 系统自带的api
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework.Utility.WINAPI
{
    public static class ImportWinApi
    {
        #region shell32.dll SHFILE 操作文件

        /// <summary>
        /// Shell文件操作数据类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class SHFILEOPSTRUCT
        {
            /// <summary>
            /// 文件句柄
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// 设置操作方式，移动：FO_MOVE，复制：FO_COPY，删除：FO_DELETE
            /// </summary>
            public wFunc wFunc;
            /// <summary>
            /// 源文件路径
            /// </summary>
            public string pFrom;
            /// <summary>
            /// 目标文件路径
            /// </summary>
            public string pTo;
            /// <summary>
            /// 允许恢复
            /// </summary>
            public FILEOP_FLAGS fFlags;
            /// <summary>
            /// 监测有无中止
            /// </summary>
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            /// <summary>
            /// 设置标题
            /// </summary>
            public string lpszProgressTitle;
        }

        /// <summary>
        /// 文件操作方式
        /// </summary>
        private enum wFunc
        {
            /// <summary>
            /// 移动
            /// </summary>
            FO_MOVE = 0x0001,
            /// <summary>
            /// 复制
            /// </summary>
            FO_COPY = 0x0002,
            /// <summary>
            /// 删除
            /// </summary>
            FO_DELETE = 0x0003,
            /// <summary>
            /// 重命名
            /// </summary>
            FO_RENAME = 0x0004
        }

        /// <summary>
        /// fFlags枚举值
        /// </summary>
        private enum FILEOP_FLAGS
        {

            ///<summary>
            ///pTo 指定了多个目标文件，而不是单个目录 
            ///</summary>
            FOF_MULTIDESTFILES = 0x1,
            ///<summary>
            ///不再使用
            ///</summary>
            FOF_CONFIRMMOUSE = 0x2,
            ///<summary>
            ///不显示一个进度对话框
            ///</summary>
            FOF_SILENT = 0x4,
            ///<summary>
            ///碰到有抵触的名字时，自动分配前缀
            ///</summary>
            FOF_RENAMEONCOLLISION = 0x8,
            ///<summary>
            ///不对用户显示提示
            ///</summary>
            FOF_NOCONFIRMATION = 0x10,
            ///<summary>
            ///填充 hNameMappings 字段，必须使用 SHFreeNameMappings 释放
            ///</summary>
            FOF_WANTMAPPINGHANDLE = 0x20,
            ///<summary>
            ///允许撤销
            ///</summary>
            FOF_ALLOWUNDO = 0x40,
            ///<summary>
            ///使用 *.* 时, 只对文件操作
            ///</summary>
            FOF_FILESONLY = 0x80,
            ///<summary>
            ///简单进度条，意味着不显示文件名。
            ///</summary>
            FOF_SIMPLEPROGRESS = 0x100,
            ///<summary>
            ///建新目录时不需要用户确定
            ///</summary>
            FOF_NOCONFIRMMKDIR = 0x200,
            ///<summary>
            ///不显示出错用户界面
            ///</summary>
            FOF_NOERRORUI = 0x400,
            ///<summary>
            /// 不复制 NT 文件的安全属性
            ///</summary>
            FOF_NOCOPYSECURITYATTRIBS = 0x800,
            ///<summary>
            /// 不递归目录
            ///</summary>
            FOF_NORECURSION = 0x1000,
            ///<summary>
            ///不将连接文件移动为组。只移动指定的文件。
            ///</summary>
            FOF_NO_CONNECTED_ELEMENTS = 0x2000,
            ///<summary>
            ///在删除操作时如果一个文件被销毁，则发送一个警告
            ///</summary>
            FOF_WANTNUKEWARNING = 0x4000,
            ///<summary>
            ///解析为一个对象，不是一个容器
            ///</summary>
            FOF_NORECURSEREPARSE = 0x8000,

        }
        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation(SHFILEOPSTRUCT lpFileOp);

        #endregion

    }
}
