namespace _36JI_
{
    using Jayrock.Json;
    using System;

    internal class GNations
    {
        private GNation[] m_Nations = new GNation[12];
        private static GNations s_Nations = new GNations();

        private GNations()
        {
            for (int i = 0; i < 12; i++)
            {
                this.m_Nations[i] = new GNation(i + 1);
            }
        }

        public GNation GetNation(int id)
        {
            return this.m_Nations[id - 1];
        }

        public bool Update(JsonObject ObjNations)
        {
            bool flag = false;
            JsonArray array = (JsonArray) ObjNations["body"];
            if (array == null)
            {
                return flag;
            }
            foreach (JsonObject obj2 in array)
            {
                int num = int.Parse(obj2["id"] as string);
                this.m_Nations[num - 1].Update(obj2);
            }
            return true;
        }

        public bool UpdateNationCity(JsonObject ObjCityInfo)
        {
            bool flag = false;
            ObjCityInfo = (JsonObject) ObjCityInfo["body"];
            JsonArray array = (JsonArray) ObjCityInfo["cityDtos"];
            if (array == null)
            {
                return flag;
            }
            for (int i = 0; i < 12; i++)
            {
                this.m_Nations[i].ClearCity();
            }
            foreach (JsonObject obj2 in array)
            {
                GCity city = new GCity(obj2);
                if ((city.CityNationId >= 1) && (city.CityNationId <= 12))
                {
                    this.m_Nations[city.CityNationId - 1].AddCity(city);
                }
            }
            return true;
        }

        public GNation[] AllNations
        {
            get
            {
                return this.m_Nations;
            }
        }

        public static GNations theNations
        {
            get
            {
                return s_Nations;
            }
        }
    }
}

