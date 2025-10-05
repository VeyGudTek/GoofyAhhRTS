using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Source.Shared.Utilities;

namespace Source.GamePlay.Services.Unit
{
    public class UnitManagerService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        GameObject TempUnit;

        private const float SpawnRayYOrigin = 100f;
        private const string EnvironmentLayerName = "Environment";
        private List<UnitService> Units { get; set; } = new List<UnitService>();
        private List<UnitService> ManuallySelectedUnits = new List<UnitService>();

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
                Units.Add(unitService);
                unitService.PlayerId = playerId;
            }
        }

        public void SelectUnit(UnitService selectedUnit, bool deselectPrevious)
        {
            if (deselectPrevious)
            {
                ManuallySelectedUnits.Clear();
                foreach (UnitService unit in Units)
                {
                    unit.DeSelect();
                }
            }
            
            if (selectedUnit != null)
            {
                if (selectedUnit.Selected)
                {
                    ManuallySelectedUnits.Remove(selectedUnit);
                    selectedUnit.DeSelect();
                }
                else
                {
                    ManuallySelectedUnits.Add(selectedUnit);
                    selectedUnit.Select();
                }
            }
        }

        public void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd, bool deselectUnits)
        {
            IEnumerable<UnitService> unitsToSelect = Units.Where(u => 
                CheckSelectArea(u.GetPosition(), selectionStart, selectionEnd) &&
                u.PlayerId == playerId
            );

            if (deselectUnits)
            {
                foreach (UnitService unit in Units.Where(u => !ManuallySelectedUnits.Contains(u) && !unitsToSelect.Contains(u)))
                {
                    unit.DeSelect();
                }
            }

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

            float stoppingDistance = unitsToMove.Aggregate(0f, 
                (total, currUnit) => total + currUnit.GetArea(), 
                total => Mathf.Sqrt(total / 4f)
            );

            foreach (UnitService unit in unitsToMove)
            {
                unit.MoveUnit(destination, stoppingDistance);
            }
        }
    }
}

