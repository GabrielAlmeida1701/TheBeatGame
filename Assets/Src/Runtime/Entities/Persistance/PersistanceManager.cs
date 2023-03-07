using Hypergame.Entities.PlayerEntity;
using UnityEngine;

namespace Hypergame.Entities.Persistance
{
    public class PersistanceManager
    {
        private static readonly string PlayerDataKey = "PlayerData";

        public static PlayerData LoadPlayerData(PlayerData initialValue)
        {
            PlayerData data;
            if (!PlayerPrefs.HasKey(PlayerDataKey))
            {
                data = initialValue.Clone();
                PlayerPrefs.SetString(PlayerDataKey, JsonUtility.ToJson(data));
            }
            else data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(PlayerDataKey));

#if UNITY_EDITOR
            if (data.stackLimit < initialValue.stackLimit)
            {
                data = initialValue.Clone();
                PlayerPrefs.SetString(PlayerDataKey, JsonUtility.ToJson(data));
            }
#endif

            return data;
        }

        public static void SavePlayerData(PlayerData data)
        {
            PlayerPrefs.SetString(PlayerDataKey, JsonUtility.ToJson(data));
        }
    }
}
