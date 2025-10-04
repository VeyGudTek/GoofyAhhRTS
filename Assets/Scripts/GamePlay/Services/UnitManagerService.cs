using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Source.GamePlay.Services
{
    public class UnitManagerService
    {
        private List<UnitService> Units { get; set; } = new List<UnitService>();

        public void SelectUnit(UnitService selectedUnit)
        {
            foreach (UnitService unit in Units.Where(u => u.Selected))
            {
                unit.Selected = false;
            }
            if (selectedUnit != null)
            {
                selectedUnit.Selected = true;
            }
        }

        public void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd)
        {
            IEnumerable<UnitService> unitsToSelect = Units.Where(u => 
                CheckSelectArea(u.GetPosition(), selectionStart, selectionEnd) &&
                u.PlayerId == playerId
            );

            foreach (UnitService unit in unitsToSelect)
            {
                unit.Selected = true;
            }
        }

        private static bool CheckSelectArea(Vector3 unit, Vector3 selectionStart, Vector3 selectionEnd)
        {
            if (unit.x < selectionStart.x || unit.x > selectionEnd.x)
            {
                return false;
            } 
            if (unit.z < selectionStart.z || unit.z > selectionEnd.z)
            {
                return false;
            }
            return true;
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

