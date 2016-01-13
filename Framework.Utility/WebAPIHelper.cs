using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Framework.Utility
{
    public partial class WebAPIHelper
    {
        #region 构建一个httt请求以获取目标链接的cookies
        /// <SUMMARY>  
        /// 构建一个httt请求以获取目标链接的cookies,需要传入目标的登录地址和相关的post信息,返回完成登录的cookies,以及返回的html内容  
        /// </SUMMARY>  
        /// <PARAM name="url">登录页面的地址</PARAM>  
        /// <PARAM name="post">post信息</PARAM>  
        /// <PARAM name="strHtml">输出的html代码</PARAM>  
        /// <PARAM name="rppt">请求的标头所需要的相关属性设置</PARAM>  
        /// <RETURNS>请求完成后的cookies</RETURNS>  
        public static CookieCollection GetCookie(string url, RequestParam rppt, out string strHtml)
        {
            CookieCollection ckclReturn = new CookieCollection();
            CookieContainer cc = new CookieContainer();
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;
            Stream stream;

            hwRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            hwRequest.CookieContainer = cc;
            if (rppt != null)
            {
                hwRequest.Accept = rppt.Accept;
                hwRequest.ContentType = rppt.ContentType;
                hwRequest.UserAgent = rppt.UserAgent;
                hwRequest.Method = rppt.Method;
                hwRequest.ContentLength = rppt.PostData.Length;
                //写入标头  

                stream = hwRequest.GetRequestStream();
                stream.Write(rppt.PostData, 0, rppt.PostData.Length);
                stream.Close();
            }
            //发送请求获取响应内容  
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            }
            catch
            {
                strHtml = "";
                return ckclReturn;
            }
            stream = hwResponse.GetResponseStream();
            StreamReader sReader = new StreamReader(stream, Encoding.UTF8);
            strHtml = sReader.ReadToEnd();
            sReader.Close();
            stream.Close();
            //获取缓存内容  
            ckclReturn = hwResponse.Cookies;
            return ckclReturn;
        }

        #endregion

        #region 根据已经获取的有效cookies来获取目标链接的内容

        /// <SUMMARY>  
        /// 根据已经获取的有效cookies来获取目标链接的内容  
        /// </SUMMARY>  
        /// <PARAM name="strUri">目标链接的url</PARAM>  
        ///<PARAM name="post">post的byte信息</PARAM>  
        /// <PARAM name="ccl">已经获取到的有效cookies</PARAM>  
        /// <PARAM name="rppt">头属性的相关设置</PARAM>  
        /// <RETURNS>目标连接的纯文本:"txt/html"</RETURNS>  
        public static string GetHtmlByCookies(string strUri, byte[] post, CookieCollection ccl, RequestParam rppt)
        {
            CookieContainer cc = new CookieContainer();
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            //构建即将发送的包头		 
            hwRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(strUri));
            cc.Add(ccl);
            hwRequest.CookieContainer = cc;
            hwRequest.Accept = rppt.Accept;
            hwRequest.ContentType = rppt.ContentType;
            hwRequest.UserAgent = rppt.UserAgent;
            hwRequest.Method = rppt.Method;
            hwRequest.ContentLength = post.Length;
            //写入post信息  
            Stream stream;
            stream = hwRequest.GetRequestStream();
            stream.Write(post, 0, post.Length);
            stream.Close();
            //发送请求获取响应内容  
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            }
            catch
            {
                return "";
            }

            stream = hwResponse.GetResponseStream();
            StreamReader sReader = new StreamReader(stream, Encoding.Default);
            string strHtml = sReader.ReadToEnd();
            sReader.Close();
            stream.Close();

            //返回值			
            return strHtml;
        }


        #endregion

        #region 根据泛型来构建字符串用于post 
        /// <summary>  
        /// 根据泛型来构建字符串用于post  
        /// </summary>  
        /// <param name="dir">带有键值对的泛型</param>  
        /// <returns>构建完毕的字符串</returns>  
        public static byte[] CreatePostData(Dictionary<string, string> dir)
        {
            StringBuilder strPost = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dir)
            {
                strPost.Append(kvp.Key);
                strPost.Append('=');
                if (!string.IsNullOrWhiteSpace(kvp.Value))
                {
                    strPost.Append(System.Web.HttpUtility.UrlEncode(kvp.Value));
                }
                strPost.Append('&');
            }
            return CreatePostData(strPost.ToString().TrimEnd('&'));
        }

        public static byte[] CreatePostData(string input)
        {
            return Encoding.Default.GetBytes(input);
        }
        #endregion

        #region 向指定uri发起GET请求

        /// <summary>
        /// 向指定uri发起GET请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GET(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            string s = wc.DownloadString(url);
            s = HttpUtility.UrlDecode(s);
            return s;
        }

        #endregion

    }
    #region HttpRequest辅助类

    /// <SUMMARY>  
    /// httpwebrequest类中的一些属性的集合  
    /// </SUMMARY>  
    public class RequestParam
    {
        /// <SUMMARY>  
        /// 获取或设置request类中的Accept属性  
        /// 用以设置接受的文件类型  
        /// </SUMMARY>			  
        public string Accept { get; set; }

        /// <SUMMARY>  
        /// 获取或设置request类中的ContentType属性  
        /// 用以设置请求的媒体类型  
        /// </SUMMARY>			
        public string ContentType { get; set; }

        /// <SUMMARY>  
        /// 获取或设置request类中的UserAgent属性  
        /// 用以设置请求的客户端信息  
        /// </SUMMARY>  
        public string UserAgent { get; set; }

        /// <SUMMARY>  
        /// 获取或设置request类中的Method属性  
        /// 可以将 Method 属性设置为任何 HTTP 1.1 协议谓词：GET、HEAD、POST、PUT、DELETE、TRACE 或 OPTIONS。  
        /// 如果 ContentLength 属性被设置为 -1 以外的任何值，则必须将 Method 属性设置为上载数据的协议属性。  
        /// </SUMMARY>			  
        public string Method { get; set; }

        /// <summary>
        /// 发送的数据
        /// </summary>
        public byte[] PostData { get; set; }
    }

    #endregion
}
