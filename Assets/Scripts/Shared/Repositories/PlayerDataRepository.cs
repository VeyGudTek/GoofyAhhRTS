using Source.Shared.Services;

namespace Source.Shared.Repositories
{
    public class PlayerData 
    {
        public PlayerData Clone() => (PlayerData)MemberwiseClone();
    }
    public class PlayerDataRepository
    {
        private PlayerData PlayerData { get; set; }
        private const string FileName = "playerData";

        public PlayerDataRepository()
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

