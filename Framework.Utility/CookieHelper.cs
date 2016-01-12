/*
 * 功能：Cookie操作类
 * 作者：单氐楠
 * 时间：2015年10月21日
 * 注意：如无必要请勿修改，谢谢！
 * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Framework.Utility
{
    public class CookieHelper
    {
        #region 设置cookie
      
        [DebuggerStepThrough]
        public static void SetCookie(string name, Dictionary<string, string> values)
        {
            SetCookie(name, values, null, null);
        }

        [DebuggerStepThrough]
        public static void SetCookie(string name, Dictionary<string, string> values, string domain)
        {
            SetCookie(name, values, domain, null);
        }
        [DebuggerStepThrough]
        public static void SetCookie(string name, Dictionary<string, string> values, DateTime expires)
        {
            SetCookie(name, values, null, expires);
        }
        [DebuggerStepThrough]
        public static void SetCookie(string name, Dictionary<string, string> values, string domain, DateTime? expires)
        {
            HttpCookie cookie = new HttpCookie(name);

            foreach (string k in values.Keys)
            {
                cookie.Values.Add(k, HttpUtility.UrlEncode(DESEncrypt.Encrypt(values[k]), Encoding.UTF8));
            }

            if (expires != null)
            {
                cookie.Expires = (DateTime)expires;
            }

            if (!string.IsNullOrEmpty(domain)) cookie.Domain = domain;
            cookie.HttpOnly = true;

            HttpContext context = HttpContext.Current;
            context.Response.Cookies.Add(cookie);
        }

        #endregion

        #region 获取cookie
        
      
        /// <summary>
        /// 获取cookie(已解密)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetCookie(string name, string key)
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            string valu = "";
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                valu = DESEncrypt.Decrypt(HttpUtility.UrlDecode(cookie.Values[key], Encoding.UTF8));
            }
            return valu;
        }
        #endregion

        #region 移除cookie
        
        [DebuggerStepThrough]
        public static void RemoveCookie(string name)
        {
            SetCookie(name, new Dictionary<string, string>(), "", DateTime.Now.AddDays(-1));
        }
        /// <summary>
        /// 将要删除的二维Cookie，写成Dictionary形式传入
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <remarks>没有基于SetCookie方法</remarks>
        public static void RemoveCookie(string name, List<string> values)
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[name];
            //List<string> cookiekeys = Utils.StrArrayToList(cookie.Values.AllKeys);
            foreach (string k in values)
            {
                //if (cookiekeys.Contains(k))
                cookie.Values.Remove(k);
            }
            context.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 清除所有cookie
        /// </summary>
        public static void RemoveAllCookies()
        {
            HttpContext context = HttpContext.Current;
            foreach (string cookieKey in context.Request.Cookies.AllKeys)
            {
                context.Response.Cookies[cookieKey].Expires = DateTime.Now.AddDays(-1);
            }
        }
        #endregion

        #region 修改cookie
        
        /// <summary>
        /// 修改cookie,将要修改的二维Cookie，写成Dictionary形式传入
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <remarks>没有基于SetCookie方法</remarks>
        public static void UpdateCookie(string name, Dictionary<string, string> values)
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[name];
            //List<string> cookiekeys = Utils.StrArrayToList(cookie.Values.AllKeys);
            foreach (string k in values.Keys)
            {
                //if (cookiekeys.Contains(k))
                cookie.Values[k] = HttpUtility.UrlEncode(DESEncrypt.Encrypt(values[k]), Encoding.UTF8);
            }
            context.Response.Cookies.Add(cookie);
        }
        #endregion
    }
}
