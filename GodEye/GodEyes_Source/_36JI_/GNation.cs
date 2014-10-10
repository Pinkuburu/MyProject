namespace _36JI_
{
    using Jayrock.Json;
    using System;
    using System.Collections.Generic;

    internal class GNation
    {
        private string bulletinInside;
        private string bulletinOutside;
        private int cityAmount;
        private int id;
        private int leaderPlayerId;
        private List<GCity> m_CityList;
        private string name;
        private int population;
        private int scaleId;
        private string shortName;

        public GNation(int ID)
        {
            this.id = ID;
            this.m_CityList = new List<GCity>();
        }

        public void AddCity(GCity city)
        {
            this.m_CityList.Add(city);
        }

        public void ClearCity()
        {
            this.m_CityList.Clear();
        }

        public GCity GetCity(int cid)
        {
            foreach (GCity city2 in this.m_CityList)
            {
                if (city2.CityID == cid)
                {
                    return city2;
                }
            }
            return null;
        }

        public GCity GetCity(string CityName)
        {
            foreach (GCity city2 in this.m_CityList)
            {
                if (string.Compare(CityName, city2.CityName) == 0)
                {
                    return city2;
                }
            }
            return null;
        }

        public void Update(JsonObject ObjNation)
        {
            this.bulletinInside = ObjNation["bulletinInside"] as string;
            this.bulletinOutside = ObjNation["bulletinOutside"] as string;
            this.name = ObjNation["name"] as string;
            this.shortName = ObjNation["shortName"] as string;
            this.cityAmount = int.Parse(ObjNation["cityAmount"] as string);
            this.id = int.Parse(ObjNation["id"] as string);
            this.leaderPlayerId = int.Parse(ObjNation["leaderPlayerId"] as string);
            this.population = int.Parse(ObjNation["population"] as string);
            this.scaleId = int.Parse(ObjNation["scaleId"] as string);
        }

        public List<GCity> AllCity
        {
            get
            {
                return this.m_CityList;
            }
        }

        public string NationBulletinInside
        {
            get
            {
                return this.bulletinInside;
            }
        }

        public string NationBulletinOutside
        {
            get
            {
                return this.bulletinOutside;
            }
        }

        public int NationCityAmount
        {
            get
            {
                return this.cityAmount;
            }
        }

        public int NationID
        {
            get
            {
                return this.id;
            }
        }

        public string NationName
        {
            get
            {
                return this.name;
            }
        }

        public string NationShortName
        {
            get
            {
                return this.shortName;
            }
        }
    }
}

