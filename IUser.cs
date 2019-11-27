using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.IO;
using System.ServiceModel.Activation;
using GameConsole.Classes;
using static GameConsole.Classes.UserBClass;
using static GameConsole.Classes.inputclasses.UserIN;

namespace GameConsole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUser" in both code and config file together.
    [ServiceContract(Namespace = "IUser/JSONData")]
    public interface IUser
    {
        /// <summary>
        /// User change password
        /// </summary>
        /// <param name="changePwd"></param>
        /// <returns></returns>
        [OperationContract(Name = "change_pwd")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "change_pwd")]
        ChangePwdCL Change_Pwd(Change_PwdIN changePwd);

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="USER_LOGIN"></param>
        /// <returns></returns>
        [OperationContract(Name = "user_login")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "user_login")]
        LoginCL User_Login(User_LoginIN USER_LOGIN);

        /// <summary>
        /// User signup
        /// </summary>
        /// <param name="signup"></param>
        /// <returns></returns>
        [OperationContract(Name = "signup")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "signup")]
        SignupCL Signup(SignupIN signup);
    }
}
