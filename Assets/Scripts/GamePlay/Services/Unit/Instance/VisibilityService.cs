using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class VisibilityService : MonoBehaviour
    {
        //Temporary Solution 
        [SerializeField]
        private GameObject VisibilityIndicator;

        public void SetVisability(bool visible)
        {
            VisibilityIndicator.SetActive(visible);
        }
    }
}

