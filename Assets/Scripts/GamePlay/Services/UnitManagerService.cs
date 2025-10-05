using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Source.Shared.Utilities;

namespace Source.GamePlay.Services
{
    public class UnitManagerService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        GameObject TempUnit;

        private const float SpawnRayYOrigin = 100f;
        private const string EnvironmentLayerName = "Environment";

        private List<UnitService> Units { get; set; } = new List<UnitService>();

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void SpawnUnit(Guid playerId, Vector2 spawnLocation)
        {
            if (TempUnit == null) return;

            RaycastHit hit;
            int layerMaskToHit = LayerMask.GetMask(EnvironmentLayerName);
            Vector3 origin = new Vector3(spawnLocation.x, SpawnRayYOrigin, spawnLocation.y);

            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, layerMaskToHit))
            {
                GameObject newUnit = Instantiate(TempUnit, hit.point, Quaternion.identity, this.transform);
                UnitService unitService = newUnit.GetComponent<UnitService>();
                Units.Add(newUnit.GetComponent<UnitService>());
                unitService.PlayerId = playerId;
            }
        }

        public void SelectUnit(UnitService selectedUnit)
        {
            foreach (UnitService unit in Units)
            {
                unit.DeSelect();
            }
            if (selectedUnit != null)
            {
                selectedUnit.Select();
            }
        }

        public void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd)
        {
            foreach (UnitService unit in Units)
            {
                unit.DeSelect();
            }

            IEnumerable<UnitService> unitsToSelect = Units.Where(u => 
                CheckSelectArea(u.GetPosition(), selectionStart, selectionEnd) &&
                u.PlayerId == playerId
            );

            foreach (UnitService unit in unitsToSelect)
            {
                unit.Select();
            }
        }

        private static bool CheckSelectArea(PositionDto unitPosition, Vector3 selectionStart, Vector3 selectionEnd)
        {
            if (!ValueIsBetween(unitPosition.Position.x, unitPosition.Radius, selectionStart.x, selectionEnd.x))
            {
                return false;
            } 
            if (!ValueIsBetween(unitPosition.Position.z, unitPosition.Radius, selectionStart.z, selectionEnd.z))
            {
                return false;
            }
            return true;
        }

        private static bool ValueIsBetween(float value, float radius, float end1, float end2)
        {
            float max = Mathf.Max(end1, end2);
            float min = Mathf.Min(end1, end2);

            return (value + radius >= min && value - radius <= max);
        }

        public void MoveUnits(Guid playerId, Vector3 destination)
        {
            IEnumerable<UnitService> unitsToMove = Units.Where(u => 
                u.PlayerId == playerId &&
                u.Selected
            );

            foreach (UnitService unit in unitsToMove)
            {
                unit.MoveUnit(destination);
            }
        }
    }
}

