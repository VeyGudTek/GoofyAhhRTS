using Source.Shared;
using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class GamePlayService : MonoBehaviour
    {

        [InitializationRequired]
        private Func<ContactDto> GetMouseWorldPoint;
        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void Initialize(Func<ContactDto> getMouseWorldPoint)
        {
            GetMouseWorldPoint = getMouseWorldPoint;
        }

        public void OnClick()
        {
            ContactDto contact = GetMouseWorldPoint?.Invoke();
            Debug.Log($"Hit GameObject: {contact.HitGameObject} | WorldPoint: {contact.Point} | GameObject: {contact.GameObject?.name}");
        }

        public void OnHold()
        {

        }

        public void OnRelease()
        {

        }
    }
}
