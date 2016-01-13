using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Framework.Utility
{
    /// <summary>
    /// HttpWebRequest帮助类
    /// </summary>
    public partial class HttpWebRequestHelper
    {
        #region GET请求
        public T HttpGet<T>(string url)
        {
            try
            {
                string retString = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        retString = streamReader.ReadToEnd().ToString();
                    }
                }
                return JsonHelper.JsonDeserializeBySingleData<T>(retString);
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求Url地址</param>
        /// <param name="postParameters">post提交参数</param>
        /// <returns></returns>
        public T HttpPost<T>(string url, Dictionary<string, string> postParameters)
        {
            try
            {
                string retString = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset:utf-8";
                //POST参数
                StringBuilder paraStrBuilder = new StringBuilder();
                foreach (string key in postParameters.Keys)
                {
                    paraStrBuilder.AppendFormat("{0}={1}&", key, postParameters[key]);
                }
                string para = paraStrBuilder.ToString();
                if (para.EndsWith("&"))
                    para = para.Remove(para.Length - 1, 1);
                //编码要跟服务器编码统一
                byte[] bt = Encoding.UTF8.GetBytes(para);
                string responseData = String.Empty;
                request.ContentLength = bt.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bt, 0, bt.Length);
                    reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        retString = streamReader.ReadToEnd().ToString();
                    }
                }
                return JsonHelper.JsonDeserializeBySingleData<T>(retString);
            }
            catch
            {
                return default(T);
            }
        }
        #endregion
    }

    /// <summary>
    /// 获取Json结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonResult<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }
}
