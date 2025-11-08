using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class StartMenuService : MonoBehaviour
    {
        [InitializationRequired]
        private StartService StartService { get; set; }

        public void InjectDependencies(StartService startService)
        {
            StartService = startService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void OnPlay()
        {
            StartService.Play();
        }

        public void OnSettings()
        {
            if (StartService == null) return;

            StartService.OpenSettings();
        }

        public void OnQuit()
        {
            StartService.Quit();
        }
    }
}
