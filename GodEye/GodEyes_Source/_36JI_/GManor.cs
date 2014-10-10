namespace _36JI_
{
    using Jayrock.Json;
    using System;

    internal class GManor
    {
        private string createTime;
        private string manorName;
        private string playerName;

        public void Update(JsonObject objManor)
        {
            this.manorName = objManor["name"] as string;
            this.playerName = objManor["playerName"] as string;
            this.createTime = objManor["createTime"] as string;
        }

        public string CreateTime
        {
            get
            {
                return this.createTime;
            }
        }

        public string ManorName
        {
            get
            {
                return this.manorName;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
        }
    }
}

