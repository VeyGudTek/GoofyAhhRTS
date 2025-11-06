using Source.Shared.Services;

namespace Source.Shared.Repositories
{
    public class PlayerData { }
    public class PlayerDataRepository
    {
        public PlayerData PlayerData { get; set; }
        private const string FileName = "playerData";

        public PlayerDataRepository()
        {
            PlayerData = DataAccessService.ReadData<PlayerData>(FileName);
        }

        public void SavePlayerData()
        {
            DataAccessService.WriteData(FileName, PlayerData);
        }
    }
}

