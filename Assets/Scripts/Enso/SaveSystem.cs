using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Enso.Characters.Player;
using UnityEngine;

namespace Enso
{
    public static class SaveSystem
    {
        private static string GetPath()
        {
            return Application.persistentDataPath + "/Player.save";
        }
        
        public static void Save(Player player)
        {
            var binaryFormatter = new BinaryFormatter();
            var path = GetPath();
            var fileStream = new FileStream(path, FileMode.Create);
            var playerData = new PlayerData(player);
            
            binaryFormatter.Serialize(fileStream, playerData);
            fileStream.Close();
        }

        public static PlayerData Load()
        {
            var path = GetPath();

            if (!File.Exists(path)) 
                return null;
            
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(path, FileMode.Open);
            var playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;
                
            fileStream.Close();
                
            return playerData;
        }
    }
}
