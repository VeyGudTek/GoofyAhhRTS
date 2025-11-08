using Source.Shared.Services;
using UnityEngine;

namespace Source.Shared.Repositories
{
    public class PlayerData 
    {
        public PlayerData Clone() => (PlayerData)MemberwiseClone();
    }
    public class PlayerDataRepository : MonoBehaviour
    {
        private PlayerData PlayerData { get; set; }
        private const string FileName = "playerData";

        private void Awake()
        {
            PlayerData = DataAccessService.ReadData<PlayerData>(FileName);
        }

        public void GetPlayerData() => PlayerData.Clone();

        public void SavePlayerData(PlayerData newPlayerData)
        {
            PlayerData = newPlayerData.Clone();
            DataAccessService.WriteData(FileName, PlayerData);
        }
    }
}

