using Source.GamePlay.Services.UI;
using TMPro;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class ResourceService : MonoBehaviour
    {
        private UnitButtonsService UnitButtonsService { get; set; }
        [SerializeField]
        private float resource = 0f;

        [SerializeField]
        private TMP_Text ResourceText;

        public void InjectDependencies(UnitButtonsService unitButtonsService)
        {
            UnitButtonsService = unitButtonsService;
        }

        private void Start()
        {
            UpdateResource();
        }

        public void ChangeResource(float value)
        {
            resource += value;
            UpdateResource();
        }

        private void UpdateResource()
        {
            ResourceText.text = $"Resources: {resource.ToString()}";
            UnitButtonsService.UpdateDisabledButtons(resource);
        }
    }
}

