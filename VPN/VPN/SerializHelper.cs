using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VPN {
    class SerializHelper {
        private static readonly string KEY_FORMAT = "{0}:{1}:{2}";
        private static readonly string DataPath = Environment.CurrentDirectory + "\\data";
        private static readonly string EnableFile = DataPath + "\\enableList.db";
        private static readonly string DisableFile = DataPath + "\\disableList.db";

        private static Dictionary<string,VPNInfoEntity> _enableList = new Dictionary<string,VPNInfoEntity>();

        internal static Dictionary<string,VPNInfoEntity> EnableList {
            get {
                return SerializHelper._enableList;
            }
            private set {
                SerializHelper._enableList = value;
            }
        }
        private static Dictionary<string,VPNInfoEntity> _disableList = new Dictionary<string,VPNInfoEntity>();

        internal static Dictionary<string , VPNInfoEntity> DisableList {
            get {
                return SerializHelper._disableList;
            }
            private set {
                SerializHelper._disableList = value;
            }
        }

        static SerializHelper() {
            if( !Directory.Exists(DataPath) ) {
                Directory.CreateDirectory(DataPath);
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs;
            if( File.Exists(EnableFile) ) {
                try {
                    fs = new FileStream(EnableFile , FileMode.Open);
                    EnableList = (Dictionary<string , VPNInfoEntity>)bf.Deserialize(fs);
                    fs.Close();
                } catch {
                }
            }
            if( File.Exists(DisableFile) ) {
                try {
                    fs = new FileStream(DisableFile , FileMode.Open);
                    DisableList = (Dictionary<string , VPNInfoEntity>)bf.Deserialize(fs);
                    fs.Close();
                } catch {
                }
            }
        }

        public static void AddEnableEntity(VPNInfoEntity entity) {
            string key = GetEntityKey(entity);
            if( !EnableList.ContainsKey(key) ) {
                EnableList.Add(key , entity);
            }
        }

        public static void RemoveEnableEntity( VPNInfoEntity entity ) {
            string key = GetEntityKey(entity);
            EnableList.Remove(key);
        }

        public static void AddDisableEntity(VPNInfoEntity entity) {
            string key = GetEntityKey(entity);
            if( !DisableList.ContainsKey(key) ) {
                DisableList.Add(key , entity);
            }
        }

        public static void RemoveDisableEntity(VPNInfoEntity entity) {
            string key = GetEntityKey(entity);
            DisableList.Remove(key);
        }

        public static string GetEntityKey( VPNInfoEntity entity ) {
            return string.Format(KEY_FORMAT , entity.Ip , entity.User , entity.Pwd);
        }

        public static void SaveEnableList() {
            if( EnableList.Count > 0 ) {
                if( !Directory.Exists(DataPath) ) {
                    Directory.CreateDirectory(DataPath);
                }

                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(EnableFile , FileMode.Create);
                bf.Serialize(fs , EnableList);
                fs.Close();
            }
        }

        public static void SaveDisableList() {
            if( DisableList.Count > 0 ) {
                if( !Directory.Exists(DataPath) ) {
                    Directory.CreateDirectory(DataPath);
                }

                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(DisableFile , FileMode.Create);
                bf.Serialize(fs , DisableList);
                fs.Close();
            }
        }

        public static void Save() {
            SaveEnableList();
            SaveDisableList();
        }
    }
}
