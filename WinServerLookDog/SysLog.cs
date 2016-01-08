using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace WinServerLookDog
{
    /// <summary>
    /// 记录系统错误日志类
    /// </summary>
    public class SysLog
    {
        #region 错误记录

        private static Exception EX = null;//错误消息
        private static string Leve = null;//错误级别
        private static string Position = null;//错误位置
        private static string SavePath = null; //保存位置

        /// <summary>
        /// 向文件中写错误日志,只写错误信息
        /// </summary>
        /// <param name="ex">错误信息</param>
        public static void WriteLog(Exception ex, string path)
        {
            SavePath = path;
            EX = ex;
            WriteDisk();//文件写入硬盘
        }

        /// <summary>
        /// 向文件中写错误日志,记录错误信息,发生错误位置
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <param name="position">发生错误位置</param>
        public static void WriteLog(Exception ex, string position, string path)
        {
            SavePath = path;
            EX = ex;
            Position = position;
            WriteDisk();//文件写入硬盘
        }

        /// <summary>
        /// 向文件中写错误日志,记录错误信息，发生错误位置，错误级别
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <param name="position">发生错误位置</param>
        /// <param name="type">错误级别</param>
        public static void WriteLog(Exception ex, string position, string leve, string path)
        {
            SavePath = path;
            EX = ex;
            Leve = leve;
            Position = position;
            WriteDisk();//文件写入硬盘
        }

        /// <summary>
        /// 文件写入硬盘
        /// </summary>
        /// <param name="str"></param>
        private static void WriteDisk()
        {
            FileStream fs = null;
            try
            {
                string str = "时间:" + System.DateTime.Now.ToString() + "\t";

                if (EX != null)
                {
                    str += "错误消息:" + EX.Message + "\t";

                    if (EX.HelpLink != null)
                    {
                        str += "与此关联的文件:" + EX.HelpLink + "\t";
                    }

                    str += "错误消息字符串表示:" + EX.StackTrace + "\t";
                }

                if (Position != null)
                {
                    str += "错误位置:" + Position + "\t";
                }

                if (Leve != null)
                {
                    str += "错误级别:" + Leve + "\t";
                }

                str += "\r\n\r\n\r\n";
                string path = SavePath + "/Logs/ErrorLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(SavePath + "/Logs/ErrorLogs/" + DateTime.Now.ToString("yyyyMM") + "/");
                    File.Create(path).Close();
                }

                //字符串写入硬盘
                byte[] bytData = System.Text.Encoding.Default.GetBytes(str);
                fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 2048);
                fs.Write(bytData, 0, bytData.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }

                EX = null;
                Position = null;
                Leve = null;
            }
        }

        /// <summary>
        /// 文件写入硬盘 操作日志
        /// </summary>
        /// <param name="str"></param>
        public static void WriteOptDisk(string info, string paths)
        {
            SavePath = paths;
            FileStream fs = null;
            try
            {
                string str = "时间:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms") + "\t";

                str += "操作信息:" + info + "\t";
                str += "\r\n\r\n\r\n";
                string path = SavePath + "/Logs/OptLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(SavePath + "/Logs/OptLogs/" + DateTime.Now.ToString("yyyyMM") + "/");
                    File.Create(path).Close();
                }

                //字符串写入硬盘
                byte[] bytData = System.Text.Encoding.Default.GetBytes(str);
                fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 2048);
                fs.Write(bytData, 0, bytData.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        #endregion


        //.....
        /// <summary>
        /// 文件写入硬盘 服务日志
        /// </summary>
        /// <param name="str"></param>
        public static void WriteServiceDisk(string info,string level, string paths)
        {
            SavePath = paths;
            FileStream fs = null;
            try
            {
                string str = "时间:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms") + "\t";
                str += "级别：" + level + "\t";
                str += "信息:" + info + "\t";
                str += "\r\n\r\n\r\n";
                string path = SavePath + "/Logs/ServiceLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(SavePath + "/Logs/ServiceLogs/" + DateTime.Now.ToString("yyyyMM") + "/");
                    File.Create(path).Close();
                }

                //字符串写入硬盘
                byte[] bytData = System.Text.Encoding.Default.GetBytes(str);
                fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 2048);
                fs.Write(bytData, 0, bytData.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 文件写入硬盘 异常日志
        /// </summary>
        /// <param name="str"></param>
        public static void WriteAbnormalDisk(string info, string level, string paths)
        {
            SavePath = paths;
            FileStream fs = null;
            try
            {
                string str = "时间:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms") + "\t";
                str += "级别：" + level + "\t";
                str += "信息:" + info + "\t";
                str += "\r\n\r\n\r\n";
                string path = SavePath + "/Logs/AbnormalLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(SavePath + "/Logs/AbnormalLogs/" + DateTime.Now.ToString("yyyyMM") + "/");
                    File.Create(path).Close();
                }

                //字符串写入硬盘
                byte[] bytData = System.Text.Encoding.Default.GetBytes(str);
                fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 2048);
                fs.Write(bytData, 0, bytData.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 读取服务日志
        /// </summary>
        /// <returns></returns>
        public static DataTable ReadServiceLog(string path)
        {
            try
            {
                DataTable dt = CreateTable("serviceLog");
                string log_path = path + "/Logs/ServiceLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";
                if(File.Exists(log_path))
                {
                    string text="";
                    string[] content;
                    using(FileStream fs=new FileStream(log_path,FileMode.Open,FileAccess.Read))
                    {
                       using(StreamReader sr=new StreamReader(fs,Encoding.GetEncoding("GB2312")))
                       {
                           int i = 0;
                           while (!sr.EndOfStream)
                           {
                              
                               text = sr.ReadLine();
                               if (!string.IsNullOrEmpty(text))
                               {
                                   content = text.Split('\t');
                                   dt.Rows.Add(content[0].Substring(3), content[1].Substring(3), content[2].Substring(3));
                                   i++;
                               }
                           }
                       }
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 读取文件  异常日志
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable ReadAbnormalLog(string path)
        {
            try
            {
                DataTable dt = CreateTable("abnormalLog");
                string log_path = path + "/Logs/AbnormalLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";
                if (File.Exists(log_path))
                {
                    string text = "";
                    string[] content;
                    using (FileStream fs = new FileStream(log_path, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("GB2312")))
                        {
                            while (!sr.EndOfStream)
                            {

                                text = sr.ReadLine();
                                if (!string.IsNullOrEmpty(text))
                                {
                                    content = text.Split('\t');
                                    dt.Rows.Add(content[0].Substring(3), content[1].Substring(3), content[2].Substring(3));
                                }
                            }
                        }
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 上传记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void UploadLog(string info,string paths)
        {
            SavePath = paths;
            FileStream fs = null;
            try
            {
                string str = "时间:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms") + "\t";
                // str += "级别：" + level + "\t";
                str += "信息:" + info + "\t";
                str += "\r\n\r\n\r\n";
                string path = SavePath + "/Logs/UploadLogs/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("yyyyMMdd") + "Log.db";

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(SavePath + "/Logs/UploadLogs/" + DateTime.Now.ToString("yyyyMM") + "/");
                    File.Create(path).Close();
                }

                //字符串写入硬盘
                byte[] bytData = System.Text.Encoding.Default.GetBytes(str);
                fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 2048);
                fs.Write(bytData, 0, bytData.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        private static DataTable CreateTable(string tableName)
        {
            DataTable dt_outcome = new DataTable(tableName);
            dt_outcome.Columns.Add("time", typeof(string));
            dt_outcome.Columns.Add("level", typeof(string));
            dt_outcome.Columns.Add("info", typeof(string));
            return dt_outcome;
        }
        
    }
}
