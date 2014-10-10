namespace _36JI_
{
    using Jayrock.Json;
    using System;

    internal class GArmy
    {
        private int food;
        private string heroName;
        private int level;
        private int nationID;
        private int playerId;
        private string playerName;
        private int pos1Amount;
        private int pos1Id;
        private int pos2Amount;
        private int pos2Id;
        private int pos3Amount;
        private int pos3Id;
        private int pos4Amount;
        private int pos4Id;
        private int pos5Amount;
        private int pos5Id;
        private int totalAmount;

        public void UpdateCityAmy(JsonObject ObjCityArmy)
        {
            this.heroName = ObjCityArmy["name"] as string;
            this.playerName = ObjCityArmy["playerName"] as string;
            this.level = int.Parse(ObjCityArmy["level"] as string);
            this.playerId = int.Parse(ObjCityArmy["playerId"] as string);
            this.food = int.Parse(ObjCityArmy["food"] as string);
            this.pos1Amount = int.Parse(ObjCityArmy["pos1Amount"] as string);
            this.pos2Amount = int.Parse(ObjCityArmy["pos2Amount"] as string);
            this.pos3Amount = int.Parse(ObjCityArmy["pos3Amount"] as string);
            this.pos4Amount = int.Parse(ObjCityArmy["pos4Amount"] as string);
            this.pos5Amount = int.Parse(ObjCityArmy["pos5Amount"] as string);
            this.totalAmount = (((this.pos1Amount + this.pos2Amount) + this.pos3Amount) + this.pos4Amount) + this.pos5Amount;
            this.pos1Id = int.Parse(ObjCityArmy["pos1Id"] as string);
            this.pos2Id = int.Parse(ObjCityArmy["pos2Id"] as string);
            this.pos3Id = int.Parse(ObjCityArmy["pos3Id"] as string);
            this.pos4Id = int.Parse(ObjCityArmy["pos4Id"] as string);
            this.pos5Id = int.Parse(ObjCityArmy["pos5Id"] as string);
        }

        public void UpdateWarArmy(JsonObject ObjWarArmy)
        {
            this.heroName = ObjWarArmy["heroName"] as string;
            this.playerName = ObjWarArmy["playerName"] as string;
            this.level = int.Parse(ObjWarArmy["level"] as string);
            this.playerId = int.Parse(ObjWarArmy["playerId"] as string);
            this.food = int.Parse(ObjWarArmy["food"] as string);
            this.nationID = int.Parse(ObjWarArmy["nationId"] as string);
            JsonArray array = (JsonArray) ObjWarArmy["armys"];
            foreach (JsonObject obj2 in array)
            {
                switch (int.Parse(obj2["pos"] as string))
                {
                    case 1:
                    {
                        this.pos1Amount = int.Parse(obj2["amount"] as string);
                        this.pos1Id = int.Parse(obj2["id"] as string);
                        continue;
                    }
                    case 2:
                    {
                        this.pos2Amount = int.Parse(obj2["amount"] as string);
                        this.pos2Id = int.Parse(obj2["id"] as string);
                        continue;
                    }
                    case 3:
                    {
                        this.pos3Amount = int.Parse(obj2["amount"] as string);
                        this.pos3Id = int.Parse(obj2["id"] as string);
                        continue;
                    }
                    case 4:
                    {
                        this.pos4Amount = int.Parse(obj2["amount"] as string);
                        this.pos4Id = int.Parse(obj2["id"] as string);
                        continue;
                    }
                    case 5:
                    {
                        this.pos5Amount = int.Parse(obj2["amount"] as string);
                        this.pos5Id = int.Parse(obj2["id"] as string);
                        continue;
                    }
                }
            }
            this.totalAmount = (((this.pos1Amount + this.pos2Amount) + this.pos3Amount) + this.pos4Amount) + this.pos5Amount;
        }

        public int Food
        {
            get
            {
                return this.food;
            }
        }

        public int HeroLevel
        {
            get
            {
                return this.level;
            }
        }

        public string HeroName
        {
            get
            {
                return this.heroName;
            }
        }

        public int NationID
        {
            get
            {
                return this.nationID;
            }
            set
            {
                this.nationID = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
            set
            {
                this.playerName = value;
            }
        }

        public int Pos1Amount
        {
            get
            {
                return this.pos1Amount;
            }
        }

        public int Pos1Id
        {
            get
            {
                return this.pos1Id;
            }
        }

        public int Pos2Amount
        {
            get
            {
                return this.pos2Amount;
            }
        }

        public int Pos2Id
        {
            get
            {
                return this.pos2Id;
            }
        }

        public int Pos3Amount
        {
            get
            {
                return this.pos3Amount;
            }
        }

        public int Pos3Id
        {
            get
            {
                return this.pos3Id;
            }
        }

        public int Pos4Amount
        {
            get
            {
                return this.pos4Amount;
            }
        }

        public int Pos4Id
        {
            get
            {
                return this.pos4Id;
            }
        }

        public int Pos5Amount
        {
            get
            {
                return this.pos5Amount;
            }
        }

        public int Pos5Id
        {
            get
            {
                return this.pos5Id;
            }
        }

        public int TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
        }
    }
}

