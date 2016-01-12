using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Utility
{
    public class DTKeys
    {

        #region 站点使用（Cookie、验证码Session等）键集合
        /// <summary>
        /// sessionKey
        /// </summary>
        public class SessionKey
        {
            /// <summary>
            /// 验证码Session键
            /// </summary>
            public const string VerifyCode = "verify_code";
            /// <summary>
            /// 系统管理员权限Session键
            /// </summary>
            public const string AdminTargetAction = "User_TargetAction";
        }

        /// <summary>
        /// 系统用户Cookie键
        /// </summary>
        public class UserCookieKey
        {
            /// <summary>
            /// Cookie一维名'SDNUser'
            /// </summary>
            public const string MainKey = "SDNUser";
            /// <summary>
            /// 用户名'F_Name'
            /// </summary>
            public const string UserName = "SDN_1";
            /// <summary>
            /// 用户密码'F_Password'
            /// </summary>
            public const string UserPassword = "SDN_2";
            /// <summary>
            /// 用户代码'F_ROLEID'
            /// </summary>
            public const string RoleID = "SDN_3";
            /// <summary>
            /// 用户角色‘F_Role’
            /// </summary>
            public const string UserRole = "SDN_4";
            /// <summary>
            /// 用户部门‘F_PARTMENTID’
            /// </summary>
            public const string DepartmentID = "SDN_5";
            /// <summary>
            /// 用户备注‘F_Remark’
            /// </summary>
            public const string UserRemark = "SDN_6";
            /// <summary>
            /// 用户ID‘F_ID’
            /// </summary>
            public const string UserID = "SDN_7";
            /// <summary>
            /// 用户登录IP
            /// </summary>
            public const string loginIp = "SDN_8";
            /// <summary>
            /// 用户MAC
            /// </summary>
            public const string loginMac = "SDN_9";
            /// <summary>
            /// 用户姓名id
            /// </summary>
            public const string PeopleId = "SDN_10";
        }

        #endregion

        #region 缓存键
        /// <summary>
        /// 缓存键
        /// </summary>
        public class CacheKey
        {
            /// <summary>
            /// 站点配置
            /// </summary>
            public const string CACHE_SITE_CONFIG = "dt_cache_site_config";
            /// <summary>
            /// 用户配置
            /// </summary>
            public const string CACHE_USER_CONFIG = "dt_cache_user_config";
            /// <summary>
            /// 客户端站点配置
            /// </summary>
            public const string CACHE_SITE_CONFIG_CLIENT = "dt_cache_site_client_config";
        }
        #endregion
    }
}
