using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class SettingsService : MonoBehaviour
    {
        [InitializationRequired]
        private GameObject MenuObject { get; set; }

        public void InjectDependencies(GameObject menuObject)
        {
            MenuObject = menuObject;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void OnBack()
        {
            if (MenuObject == null) return;

            gameObject.SetActive(false);
            MenuObject.SetActive(true);
        }
    }
}
