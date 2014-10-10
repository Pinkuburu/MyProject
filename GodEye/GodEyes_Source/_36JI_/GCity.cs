namespace _36JI_
{
    using Jayrock.Json;
    using System;
    using System.Collections.Generic;

    internal class GCity
    {
        // Fields
        private int area;
        private int[] ArmyCount;
        public bool bHasHighLevelHero;
        private string bulletin;
        public bool bUseSpy;
        private int defense;
        private int foodValue;
        private int id;
        private int ironValue;
        private int key;
        private int level;
        private bool m_bScanArmy;
        private List<GArmy> m_CityArmyList;
        private List<GArmy> m_CityHighLevelHeroList;
        private List<GManor> m_CityManorList;
        private int morale;
        private string name;
        private int nationClass;
        private int nationId;
        private static string[] s_ArmyClass = new string[] { "轻步兵({0})\r\n", "弓箭手({0})\r\n", "轻骑兵({0})\r\n", "重步兵({0})\r\n", "强弓手({0})\r\n", "重骑兵({0})\r\n", "禁卫军({0})\r\n", "弩骑兵({0})\r\n", "虎豹骑({0})\r\n", "运输兵({0})\r\n", "运输车({0})\r\n", "羽林军({0})\r\n", "战车({0})\r\n", "帝国铁骑({0})\r\n", "义军({0})\r\n" };
        private int safety;
        public int SpyArmyCount;
        public int SpyArmyNum;
        private int status;
        private int traffic;
        private int woodValue;

        // Methods
        public GCity(JsonObject ObjCity)
        {
            this.name = ObjCity["name"] as string;
            this.bulletin = ObjCity["bulletin"] as string;
            this.area = int.Parse(ObjCity["area"] as string);
            this.safety = int.Parse(ObjCity["safety"] as string);
            this.defense = int.Parse(ObjCity["defense"] as string);
            this.traffic = int.Parse(ObjCity["traffic"] as string);
            this.morale = int.Parse(ObjCity["morale"] as string);
            this.foodValue = int.Parse(ObjCity["foodValue"] as string);
            this.ironValue = int.Parse(ObjCity["ironValue"] as string);
            this.woodValue = int.Parse(ObjCity["woodValue"] as string);
            this.id = int.Parse(ObjCity["id"] as string);
            this.key = int.Parse(ObjCity["key"] as string);
            this.nationId = int.Parse(ObjCity["nationId"] as string);
            this.nationClass = int.Parse(ObjCity["nationClass"] as string);
            this.level = int.Parse(ObjCity["level"] as string);
            this.status = int.Parse(ObjCity["status"] as string);
            this.ArmyCount = new int[15];
            this.bHasHighLevelHero = false;
            this.bUseSpy = false;
        }

        private void GetArmyCount(int id, int count)
        {
            switch (id)
            {
                case 0x7d1:
                    this.ArmyCount[0] += count;
                    return;

                case 0x7d2:
                    this.ArmyCount[1] += count;
                    return;

                case 0x7d3:
                    this.ArmyCount[2] += count;
                    return;

                case 0x7d4:
                    this.ArmyCount[3] += count;
                    return;

                case 0x7d5:
                    this.ArmyCount[4] += count;
                    return;

                case 0x7d6:
                    this.ArmyCount[5] += count;
                    return;

                case 0x7d7:
                    this.ArmyCount[6] += count;
                    return;

                case 0x7d8:
                    this.ArmyCount[7] += count;
                    return;

                case 0x7d9:
                    this.ArmyCount[8] += count;
                    return;

                case 0x7da:
                    this.ArmyCount[9] += count;
                    return;

                case 0x7db:
                    this.ArmyCount[10] += count;
                    return;

                case 0x7dc:
                    this.ArmyCount[11] += count;
                    return;

                case 0x7dd:
                    this.ArmyCount[12] += count;
                    return;

                case 0x7de:
                    this.ArmyCount[13] += count;
                    return;

                case 0x7df:
                case 0x7e0:
                case 0x7e1:
                    break;

                case 0x7e2:
                    this.ArmyCount[14] += count;
                    break;

                default:
                    return;
            }
        }

        private string GetArmyInfo(List<GArmy> ArmyList)
        {
            int num2;
            int num3;
            int num4;
            int num5;
            int num6;
            int num = num2 = num3 = num4 = num5 = num6 = 0;
            for (int i = 0; i < 15; i++)
            {
                this.ArmyCount[i] = 0;
            }
            foreach (GArmy army in ArmyList)
            {
                num += army.TotalAmount;
                num2 += army.Pos1Amount;
                num3 += army.Pos2Amount;
                num4 += army.Pos3Amount;
                num5 += army.Pos4Amount;
                num6 += army.Pos5Amount;
                this.GetArmyCount(army.Pos1Id, army.Pos1Amount);
                this.GetArmyCount(army.Pos2Id, army.Pos2Amount);
                this.GetArmyCount(army.Pos3Id, army.Pos3Amount);
                this.GetArmyCount(army.Pos4Id, army.Pos4Amount);
                this.GetArmyCount(army.Pos5Id, army.Pos5Amount);
            }
            string str = string.Format("总兵力：{0}\r\n", num);
            if (num == 0)
            {
                return str;
            }
            str = str + string.Format("军队数：{0}\r\n前锋：{1} 前军：{2} 中军：{3} 后军：{4} 后勤：{5}\r\n", new object[] { ArmyList.Count, num2, num3, num4, num5, num6 }) + "-----兵种统计-----\r\n";
            for (int j = 0; j < 15; j++)
            {
                if (this.ArmyCount[j] != 0)
                {
                    str = str + string.Format(s_ArmyClass[j], this.ArmyCount[j]);
                }
            }
            return (str + "\r\n");
        }

        public string GetCityArmyInfo()
        {
            string str = string.Format("-----{0}兵力统计-----\r\n", this.name);
            if (this.bUseSpy)
            {
                str = str + string.Format("总兵力：{0}\r\n", this.SpyArmyCount) + string.Format("军队数：{0}\r\n", this.SpyArmyNum);
                if (this.bHasHighLevelHero)
                {
                    return (str + "\r\n该城市有高等级英雄，请点军队列表查看");
                }
                return (str + "\r\n该城市没有高等级英雄");
            }
            if (this.m_bScanArmy)
            {
                str = str + this.GetArmyInfo(this.m_CityArmyList);
                if (this.m_CityArmyList.Count > 0)
                {
                    str = str + "详细兵力请点 军队列表";
                }
                return str;
            }
            if (this.bHasHighLevelHero)
            {
                return (str + "无法查看兵力\r\n\r\n该城市有高等级英雄，请点军队列表查看");
            }
            return (str + "无法查看兵力\r\n\r\n建议用小号进入该国查看城市兵力");
        }

        public string GetWarArmyInfo()
        {
            if (!this.m_bScanArmy)
            {
                return "网络错误或者等待下一回合中，请稍后再查看兵力";
            }
            string str = "";
            List<GArmy>[] listArray = new List<GArmy>[12];
            foreach (GArmy army in this.m_CityArmyList)
            {
                if (listArray[army.NationID - 1] == null)
                {
                    listArray[army.NationID - 1] = new List<GArmy>();
                }
                listArray[army.NationID - 1].Add(army);
            }
            for (int i = 0; i < 12; i++)
            {
                List<GArmy> armyList = listArray[i];
                if (armyList != null)
                {
                    str = str + string.Format("-----{0}兵力统计-----\r\n", GNations.theNations.GetNation(i + 1).NationName) + this.GetArmyInfo(armyList);
                    armyList.Clear();
                }
            }
            if (this.m_CityArmyList.Count > 0)
            {
                str = str + "详细兵力请点 军队列表";
            }
            return str;
        }

        public bool IsThisCity(string strText, bool bName)
        {
            bool flag = false;
            if (bName)
            {
                if (this.name.IndexOf(strText) != -1)
                {
                    flag = true;
                }
                return flag;
            }
            if (int.Parse(strText) == this.id)
            {
                flag = true;
            }
            return flag;
        }

        public void UpdateCityArmy(JsonObject ObjCityArmy)
        {
            if (this.m_CityArmyList == null)
            {
                this.m_CityArmyList = new List<GArmy>();
            }
            else
            {
                this.m_CityArmyList.Clear();
            }
            ObjCityArmy = (JsonObject)ObjCityArmy["body"];
            JsonArray array = (JsonArray)ObjCityArmy["heroes"];
            foreach (JsonObject obj2 in array)
            {
                GArmy item = new GArmy();
                item.UpdateCityAmy(obj2);
                item.NationID = this.nationId;
                this.m_CityArmyList.Add(item);
            }
            this.m_bScanArmy = true;
        }

        public void UpdateCityHighLevelHero(JsonObject ObjHighLevelHero, string heroOwnerName)
        {
            if (this.m_CityHighLevelHeroList == null)
            {
                this.m_CityHighLevelHeroList = new List<GArmy>();
            }
            GArmy item = new GArmy();
            item.UpdateCityAmy(ObjHighLevelHero);
            item.NationID = this.nationId;
            item.PlayerName = heroOwnerName;
            this.m_CityHighLevelHeroList.Add(item);
            this.bHasHighLevelHero = true;
        }

        public void UpdateManor(JsonObject ObjManor)
        {
            if (this.m_CityManorList == null)
            {
                this.m_CityManorList = new List<GManor>();
            }
            else
            {
                this.m_CityManorList.Clear();
            }
            JsonArray array = (JsonArray)ObjManor["body"];
            if (array != null)
            {
                foreach (JsonObject obj2 in array)
                {
                    GManor item = new GManor();
                    item.Update(obj2);
                    this.m_CityManorList.Add(item);
                }
            }
        }

        public void UpdateSpyCity(JsonObject ObjSpyCity)
        {
            ObjSpyCity = (JsonObject)ObjSpyCity["body"];
            this.bUseSpy = true;
            this.SpyArmyCount = int.Parse(ObjSpyCity["armyCount"] as string);
            this.SpyArmyNum = int.Parse(ObjSpyCity["heroCount"] as string);
            this.m_bScanArmy = true;
        }

        public void UpdateWarArmy(JsonObject ObjWarArmy)
        {
            if (this.m_CityArmyList == null)
            {
                this.m_CityArmyList = new List<GArmy>();
            }
            else
            {
                this.m_CityArmyList.Clear();
            }
            ObjWarArmy = (JsonObject)ObjWarArmy["body"];
            ObjWarArmy = (JsonObject)ObjWarArmy["report"];
            if (ObjWarArmy != null)
            {
                JsonArray array = (JsonArray)ObjWarArmy["troops"];
                if (array != null)
                {
                    foreach (JsonObject obj2 in array)
                    {
                        GArmy item = new GArmy();
                        item.UpdateWarArmy(obj2);
                        this.m_CityArmyList.Add(item);
                    }
                    this.m_bScanArmy = true;
                }
            }
        }

        // Properties
        public List<GArmy> ArmyList
        {
            get
            {
                List<GArmy> cityHighLevelHeroList = null;
                if (this.bUseSpy)
                {
                    if (this.bHasHighLevelHero)
                    {
                        cityHighLevelHeroList = this.m_CityHighLevelHeroList;
                    }
                    return cityHighLevelHeroList;
                }
                if (this.m_bScanArmy)
                {
                    return this.m_CityArmyList;
                }
                if (this.bHasHighLevelHero)
                {
                    cityHighLevelHeroList = this.m_CityHighLevelHeroList;
                }
                return cityHighLevelHeroList;
            }
        }

        public int CityArea
        {
            get
            {
                return this.area;
            }
        }

        public string CityBulletin
        {
            get
            {
                return this.bulletin;
            }
        }

        public int CityDefense
        {
            get
            {
                return this.defense;
            }
        }

        public int CityID
        {
            get
            {
                return this.id;
            }
        }

        public int CityMorale
        {
            get
            {
                return this.morale;
            }
        }

        public string CityName
        {
            get
            {
                return this.name;
            }
        }

        public int CityNationId
        {
            get
            {
                return this.nationId;
            }
        }

        public int CitySafety
        {
            get
            {
                return this.safety;
            }
        }

        public int CityStatus
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        public int CityTraffic
        {
            get
            {
                return this.traffic;
            }
        }

        public List<GManor> ManorList
        {
            get
            {
                return this.m_CityManorList;
            }
        }

        public bool ScanArmy
        {
            get
            {
                return this.m_bScanArmy;
            }
            set
            {
                this.m_bScanArmy = value;
            }
        }

        public bool UseSpy
        {
            get
            {
                return this.bUseSpy;
            }
        }
    }
}

