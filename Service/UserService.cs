using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Model;
using Model.Models;

namespace Service
{
    public class UserService : BaseService, IUserService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="userPwd">User pwd.</param>
        public bool Login(string userName, string userPwd)
        {
            userPwd = SecurityHelper.getMd5Hash(userPwd);
            var userModel = DB.Queryable<SysUser>().Single(f => f.UserName == userName && f.UserPassword == userPwd);
            if (userModel != null)    //存在则登录成功, 写入Cookie  
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="email">Email.</param>
        /// <param name="userName">User name.</param>
        /// <param name="userPassword">User password.</param>
        public bool Register(string email, string userName, string userPassword)
        {
            SysUser sysUser = new SysUser();
            sysUser.UserName = userName;
            sysUser.UserEmail = email;
            sysUser.UserPassword = SecurityHelper.getMd5Hash(userPassword);
            sysUser.CreatedTime = DateTime.Now;
            var count = DB.Insertable(sysUser).ExecuteCommand();
            if (count > 0)    //存在则注册成功, 写入Cookie  
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否存在用户名称
        /// </summary>
        /// <returns><c>true</c>, if exist account was ised, <c>false</c> otherwise.</returns>
        /// <param name="account">Account.</param>
        public bool IsExistAccount(string account)
        {
            var userModel = DB.Queryable<SysUser>().Single(f => f.UserName == account);
            if (userModel != null)    //存在则登录成功, 写入Cookie  
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否存在邮箱
        /// </summary>
        /// <returns><c>true</c>, if exist email was ised, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        public bool IsExistEmail(string email)
        {
            var userModel = DB.Queryable<SysUser>().Single(f => f.UserEmail == email);
            if (userModel != null)    //存在则登录成功, 写入Cookie  
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断用户账号和邮箱是否存在
        /// </summary>
        /// <returns><c>true</c>, if exist account was ised, <c>false</c> otherwise.</returns>
        /// <param name="account">Account.</param>
        public bool CheckEmailAndAccount(string email, string account)
        {
            var userModel = DB.Queryable<SysUser>().Single(f => f.UserName == account && f.UserEmail==email);
            if (userModel != null)    //存在则登录成功, 写入Cookie  
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateEmailTokenAndExpire(string email, string account)
        {
            var userModel = DB.Queryable<SysUser>().Single(f => f.UserEmail == email);
            if (userModel != null)    //存在则登录成功, 写入Cookie  
            {
                string token = Guid.NewGuid().ToString().Replace("-","");
                userModel.EmailToken = token;
                userModel.EmailExpire = DateTime.Now;
                var t1 = DB.Updateable(userModel).UpdateColumns(it => new { it.EmailToken,it.EmailExpire }).ExecuteCommand();
                if(t1>0){
                    return true;
                }
                return false;
            }
            return false;
        }

        #region 登录使用的加密解密方法
        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public string LoginEncrypt(string encryptString, string encryptKey)
        {
            return SecurityHelper.EncryptDES(encryptString, encryptKey);
        }
        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public string LoginDecrypt(string decryptString, string encryptKey)
        {
            return SecurityHelper.DecryptDES(decryptString, encryptKey);
        }
        #endregion
    }
}
