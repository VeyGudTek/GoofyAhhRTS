using System.Collections.Generic;
using Source.GamePlay.Static.ScriptableObjects;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitVisionService : MonoBehaviour
    {
        private UnitService Self { get; set; }
        private List<UnitService> UnitsInVision = new();

        public void InjectDependencies(UnitService self, UnitData unitData)
        {
            Self = self;

            transform.localScale = new Vector3(unitData.Vision * 2f, transform.localScale.y, unitData.Vision * 2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Self == null) return;

            if (other.gameObject.TryGetComponent<UnitService>(out var newUnit))
            {
                UnitsInVision.Add(newUnit);
                Self.UnitComputerService.OnVisionEnter(newUnit);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Self == null) return;

            if (other.gameObject.TryGetComponent<UnitService>(out var newUnit))
            {
                UnitsInVision.Remove(newUnit);
                Self.UnitComputerService.OnVisionExit(UnitsInVision);
            }
        }
    }
}

