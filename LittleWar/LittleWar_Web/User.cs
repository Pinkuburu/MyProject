using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LittleWar_Web
{
    public class User
    {
        private string id;//id   
        private long systemTime;//������ʱ��   
        private int food;//ʳ��   
        private int force;//����   
        private int grade;//�ȼ�   
        private int population_limit;//�˿�   
        private int population_all;//���˿�   
        private int mp;//ħ��ֵ   

        private int loot_times;//��ٴ���   

        public string getId()
        {
            return id;
        }
        public void setId(int id)
        {
            this.id = id.ToString();
        }
        public int getFood()
        {
            return food;
        }
        public void setFood(int food)
        {
            this.food = food;
        }
        public int getForce()
        {
            return force;
        }
        public void setForce(int force)
        {
            this.force = force;
        }
        public long getSystemTime()
        {
            return systemTime;
        }
        public void setSystemTime(long systemTime)
        {
            this.systemTime = systemTime;
        }
        public int getGrade()
        {
            return grade;
        }
        public void setGrade(int grade)
        {
            this.grade = grade;
        }

        public int getPopulation_all()
        {
            return population_all;
        }
        public void setPopulation_all(int populationAll)
        {
            population_all = populationAll;
        }
        public int getMp()
        {
            return mp;
        }
        public void setMp(int mp)
        {
            this.mp = mp;
        }
        public int getPopulation_limit()
        {
            return population_limit;
        }
        public void setPopulation_limit(int populationLimit)
        {
            population_limit = populationLimit;
        }

        public void updateNoTime(object jsonu)
        {
            JObject o = JObject.Parse(jsonu.ToString());
            try
            {
                this.setId((int)o["uid"]);
                this.setFood((int)o["food"]);
                this.setForce((int)o["population"]);
                this.setPopulation_all((int)o["population_all"]);
                this.setPopulation_limit((int)o["population_limit"]);
                this.setGrade((int)o["grade"]);
                this.setMp((int)o["mp"]);
            }
            catch (JsonReaderException e)
            {
                e.ToString();
            }
        }

        public int getLoot_times()
        {
            return loot_times;
        }

        public void setLoot_times(int lootTimes)
        {
            loot_times = lootTimes;
        }

    }
}
