using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="userPwd">User pwd.</param>
        bool Login(string userName, string userPwd);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="email">Email.</param>
        /// <param name="userName">User name.</param>
        /// <param name="userPassword">User password.</param>
        bool Register(string email, string userName, string userPassword);

        /// <summary>
        /// 判断是否存在用户名称
        /// </summary>
        /// <returns><c>true</c>, if exist account was ised, <c>false</c> otherwise.</returns>
        /// <param name="account">Account.</param>
        bool IsExistAccount(string account);

        /// <summary>
        /// 判断是否存在邮箱
        /// </summary>
        /// <returns><c>true</c>, if exist email was ised, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        bool IsExistEmail(string email);

        /// <summary>
        /// 判断邮箱和帐号是否存在
        /// </summary>
        /// <returns><c>true</c>, if exist email was ised, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        bool CheckEmailAndAccount(string email,string account);

        /// <summary>
        /// 更新邮箱密钥和过期时间
        /// </summary>
        /// <returns><c>true</c>, if exist email was ised, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        bool UpdateEmailTokenAndExpire(string email,string account);

        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        string LoginEncrypt(string encryptString, string encryptKey);
        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        string LoginDecrypt(string decryptString, string encryptKey);

    }
}
