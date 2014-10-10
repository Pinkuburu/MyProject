namespace _36JI_
{
    using Jayrock.Json;
    using Jayrock.Json.Conversion;
    using System;
    using System.Drawing;
    using System.Text;

    internal class GUser : GThread
    {
        // Fields
        private bool m_bLogined;
        private bool m_bUseSpy;
        private GHTTPConnect m_HTTPConnect = new GHTTPConnect();
        private GUserInterface m_UIInterface;
        private GCity m_WorkCity;

        // Methods
        public GUser(GUserInterface UIInterface)
        {
            this.m_UIInterface = UIInterface;
            base.RunThread();
        }

        private string FormatString(string Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            string str = "";
            foreach (byte num in bytes)
            {
                if (num >= 0x80)
                {
                    str = str + string.Format("%{0:X2}", num);
                }
                else
                {
                    char ch = (char) num;
                    switch (ch)
                    {
                        case '%':
                            str = str + "%25";
                            break;

                        case '&':
                            str = str + "%26";
                            break;

                        default:
                            str = str + ch;
                            break;
                    }
                }
            }
            return str;
        }

        public Image GetValidateCodeStream(string strServer)
        {
            return this.m_HTTPConnect.GetValidateCodeStream(strServer);
        }

        public void InitUser(string Name, string PassWord, string Server, string ValidateCode)
        {
            this.m_HTTPConnect.Init(Name, PassWord, Server, ValidateCode);
            base.DoWork(GThread.EWorkType.ELogin);
        }

        private void SendMessage(string UserName, string Title, string Content)
        {
            string strformData = "action=Message%2Esend&tpname=";
            strformData = ((strformData + this.FormatString(UserName)) + "&content=" + this.FormatString(Content)) + "&tpid=0&title=" + this.FormatString(Title);
            this.m_HTTPConnect.GetHTTPData(strformData, false);
        }

        public void TestGuangGao()
        {
            for (int i = 0; i < 2; i++)
            {
                string str;
                if (i == 1)
                {
                    str = string.Format("action=Rank%2EplayerAll&type={0}&size=60&index=1", 2);
                }
                else
                {
                    str = string.Format("action=Rank%2EplayerAll&type={0}&size=60&index=1", 0);
                }
                JsonObject obj2 = (JsonObject) JsonConvert.Import(this.m_HTTPConnect.GetHTTPData(str, true));
                obj2 = (JsonObject) obj2["body"];
                JsonArray array = (JsonArray) obj2["players"];
                string title = "上帝之眼来17173看看";
                string content = "可查看任何城市的防御和兵力信息，能看到交战城市的兵力情况。免费试用，淘宝联系。本消 息自动发送，勿回复。下载地址：ftp://222.211.93.128/GodEyes.zip 淘宝地址：http://item.taobao.com/auction/item_detail.jhtml?item_id=7d98e90890db19a8a02ec3906c41e4e7&x_id=0db1";
                if (array != null)
                {
                    foreach (JsonObject obj3 in array)
                    {
                        string userName = obj3["name"] as string;
                        this.SendMessage(userName, title, content);
                    }
                }
            }
        }

        private bool Update(EUpdateType eType)
        {
            string hTTPData;
            JsonObject obj2;
            bool flag = false;
            switch (eType)
            {
                case EUpdateType.EBasic:
                    this.m_HTTPConnect.GetHTTPData("action=Main%2EgetBasicInfo&format=BIN", false);
                    return flag;

                case EUpdateType.EMap:
                    hTTPData = this.m_HTTPConnect.GetHTTPData("action=Main%2EmapInfo&format=BIN", true);
                    if (!(this.m_HTTPConnect.ErrorCode == "SUCCESS"))
                    {
                        if ("NET_ERROR" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("网络连接错误");
                            return flag;
                        }
                        if ("USER_EXPIRED" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("用户授权已过期");
                        }
                        return flag;
                    }
                    obj2 = (JsonObject) JsonConvert.Import(hTTPData);
                    GNations.theNations.UpdateNationCity(obj2);
                    return flag;

                case EUpdateType.ENations:
                    hTTPData = this.m_HTTPConnect.GetHTTPData("action=Main%2EgetNations", true);
                    if (!(this.m_HTTPConnect.ErrorCode == "SUCCESS"))
                    {
                        if ("NET_ERROR" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("网络连接错误");
                            return flag;
                        }
                        if ("USER_EXPIRED" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("用户授权已过期");
                        }
                        return flag;
                    }
                    obj2 = (JsonObject) JsonConvert.Import(hTTPData);
                    GNations.theNations.Update(obj2);
                    return flag;

                case EUpdateType.EUserInfo:
                    this.m_HTTPConnect.GetHTTPData("action=BaseInfo%2EuserInfo", true);
                    return flag;

                case EUpdateType.EHeroList:
                    this.m_HTTPConnect.GetHTTPData("action=Hero%2ElistAll", true);
                    return flag;

                case EUpdateType.EManorList:
                    this.m_HTTPConnect.GetHTTPData("action=Manor%2Elist", true);
                    return flag;

                case EUpdateType.EMyInfo:
                    this.m_HTTPConnect.GetHTTPData("action=Player%2EmyInfo", true);
                    return flag;

                case EUpdateType.EBattleInfo:
                {
                    string strformData = string.Format("action=City%2EBattle&format=BIN&cid={0}", this.m_WorkCity.CityID);
                    hTTPData = this.m_HTTPConnect.GetHTTPData(strformData, true);
                    if (string.Compare(this.m_HTTPConnect.ErrorCode, "ERR_CITY_000020", true) != 0)
                    {
                        if (this.m_HTTPConnect.ErrorCode == "SUCCESS")
                        {
                            obj2 = (JsonObject) JsonConvert.Import(hTTPData);
                            this.m_WorkCity.UpdateWarArmy(obj2);
                            return flag;
                        }
                        if ("NET_ERROR" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("网络连接错误");
                            return flag;
                        }
                        if ("USER_EXPIRED" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("用户授权已过期");
                        }
                        return flag;
                    }
                    this.m_WorkCity.CityStatus = 0;
                    this.m_UIInterface.CityStatusUpdated();
                    this.Update(EUpdateType.ECityInfo);
                    return flag;
                }
                case EUpdateType.ECityInfo:
                {
                    string str3 = string.Format("action=City%2EgetCityInner&format=BIN&cid={0}", this.m_WorkCity.CityID);
                    hTTPData = this.m_HTTPConnect.GetHTTPData(str3, true);
                    if (string.Compare(this.m_HTTPConnect.ErrorCode, "ERR_CITY_000021", true) != 0)
                    {
                        if (this.m_bUseSpy && (string.Compare(this.m_HTTPConnect.ErrorCode, "ERR_MAIN_000026", true) == 0))
                        {
                            if (this.m_UIInterface.ShowUseSpyWarning())
                            {
                                str3 = string.Format("action=City%2Espy&goldType=1&cid={0}", this.m_WorkCity.CityID);
                                hTTPData = this.m_HTTPConnect.GetHTTPData(str3, true);
                                if (this.m_HTTPConnect.ErrorCode == "SUCCESS")
                                {
                                    obj2 = (JsonObject) JsonConvert.Import(hTTPData);
                                    this.m_WorkCity.UpdateSpyCity(obj2);
                                }
                                return flag;
                            }
                            if (this.m_WorkCity.UseSpy)
                            {
                                this.m_WorkCity.ScanArmy = true;
                            }
                            return flag;
                        }
                        if (this.m_HTTPConnect.ErrorCode == "SUCCESS")
                        {
                            obj2 = (JsonObject) JsonConvert.Import(hTTPData);
                            this.m_WorkCity.UpdateCityArmy(obj2);
                            return flag;
                        }
                        if ("NET_ERROR" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("网络连接错误");
                            return flag;
                        }
                        if ("USER_EXPIRED" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("用户授权已过期");
                        }
                        return flag;
                    }
                    this.m_WorkCity.CityStatus = 1;
                    this.m_UIInterface.CityStatusUpdated();
                    this.Update(EUpdateType.EBattleInfo);
                    return flag;
                }
                case EUpdateType.ECityManorInfo:
                {
                    string str4 = string.Format("action=City%2ElistManor&cid={0}", this.m_WorkCity.CityID);
                    hTTPData = this.m_HTTPConnect.GetHTTPData(str4, true);
                    if (!(this.m_HTTPConnect.ErrorCode == "SUCCESS"))
                    {
                        if ("NET_ERROR" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("网络连接错误");
                            return flag;
                        }
                        if ("USER_EXPIRED" == this.m_HTTPConnect.ErrorCode)
                        {
                            this.m_UIInterface.WorkMessageUpdated("用户授权已过期");
                        }
                        return flag;
                    }
                    obj2 = (JsonObject) JsonConvert.Import(hTTPData);
                    this.m_WorkCity.UpdateManor(obj2);
                    return flag;
                }
            }
            return flag;
        }

        public void UpdateCity(GCity City, bool bUseSpy)
        {
            base.WaitWork();
            this.m_WorkCity = City;
            this.m_bUseSpy = bUseSpy;
            base.DoWork(GThread.EWorkType.EUpdateCityInfo);
        }

        private void UpdateHero()
        {
            string strformData = string.Format("action=Rank%2Ehero&size=200&index=1", new object[0]);
            JsonObject obj2 = (JsonObject) JsonConvert.Import(this.m_HTTPConnect.GetHTTPData(strformData, true));
            obj2 = (JsonObject) obj2["body"];
            JsonArray array = (JsonArray) obj2["heroes"];
            if (array != null)
            {
                foreach (JsonObject obj3 in array)
                {
                    int id = int.Parse(obj3["nationId"] as string);
                    string heroOwnerName = obj3["playerName"] as string;
                    strformData = string.Format("action=Hero%2Einfo&hid={0}", obj3["heroId"] as string);
                    JsonObject objHighLevelHero = (JsonObject) JsonConvert.Import(this.m_HTTPConnect.GetHTTPData(strformData, true));
                    objHighLevelHero = (JsonObject) objHighLevelHero["body"];
                    int cid = int.Parse(objHighLevelHero["cityId"] as string);
                    GCity city = GNations.theNations.GetNation(id).GetCity(cid);
                    if (city != null)
                    {
                        city.UpdateCityHighLevelHero(objHighLevelHero, heroOwnerName);
                    }
                }
            }
        }

        public void UpdateNationAndCity()
        {
            base.DoWork(GThread.EWorkType.EUpdateNationAndCity);
        }

        protected override void Working()
        {
            switch (base.m_WorkType)
            {
                case GThread.EWorkType.ELogin:
                    this.m_UIInterface.WorkMessageUpdated("正在登录...");
                    this.m_bLogined = this.m_HTTPConnect.Login();
                    if (!this.m_bLogined)
                    {
                        this.m_UIInterface.WorkMessageUpdated("登录失败,请稍后重试");
                    }
                    else
                    {
                        this.m_UIInterface.WorkMessageUpdated("登录完成");
                    }
                    goto Label_0110;

                case GThread.EWorkType.EUpdateNationAndCity:
                    this.m_UIInterface.WorkMessageUpdated("更新地图和城市...");
                    this.Update(EUpdateType.EMap);
                    this.Update(EUpdateType.ENations);
                    this.m_UIInterface.NationUpdated();
                    this.m_UIInterface.WorkMessageUpdated("更新完成");
                    goto Label_0110;

                case GThread.EWorkType.EUpdateCityInfo:
                    if (this.m_WorkCity.ScanArmy)
                    {
                        goto Label_0105;
                    }
                    this.m_UIInterface.WorkMessageUpdated("更新城市信息...");
                    if (this.m_WorkCity.CityStatus != 0)
                    {
                        this.Update(EUpdateType.EBattleInfo);
                        break;
                    }
                    this.Update(EUpdateType.ECityInfo);
                    break;

                default:
                    goto Label_0110;
            }
            this.Update(EUpdateType.ECityManorInfo);
            this.m_UIInterface.WorkMessageUpdated("更新完成");
        Label_0105:
            this.m_UIInterface.CityUpdated();
        Label_0110:
            base.m_WorkType = GThread.EWorkType.EIdle;
        }

        // Properties
        public bool Logined
        {
            get
            {
                return this.m_bLogined;
            }
        }

        public string Name
        {
            get
            {
                return this.m_HTTPConnect.Name;
            }
        }

        public string PassWord
        {
            get
            {
                return this.m_HTTPConnect.PassWord;
            }
        }

        public string Server
        {
            get
            {
                return this.m_HTTPConnect.Server;
            }
        }

        public string Session
        {
            get
            {
                return this.m_HTTPConnect.Session;
            }
        }

        public GCity WorkCity
        {
            get
            {
                return this.m_WorkCity;
            }
        }

        // Nested Types
        private enum EUpdateType
        {
            EBasic,
            EMap,
            ENations,
            EUserInfo,
            EHeroList,
            EManorList,
            EMyInfo,
            EBattleInfo,
            ECityInfo,
            ECityManorInfo,
            EPlayerInfo
        }
    }
}

