using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Source.GamePlay
{
    public class UnitManager : MonoBehaviour
    {
        private List<Unit> Units = new List<Unit>();
        
        public void SelectUnit(Unit selectedUnit)
        {
            foreach (Unit unit in Units.Where(u => u.Selected))
            {
                unit.Deselect();
            }
            if (selectedUnit != null)
            {
                selectedUnit.Select();
            }
        }

        public void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd)
        {
            IEnumerable<Unit> unitsToSelect = Units.Where(u => 
                CheckSelectArea(u.transform.position, selectionStart, selectionEnd) &&
                u.PlayerId == playerId
            );

            foreach (Unit unit in unitsToSelect)
            {
                unit.Select();
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
            IEnumerable<Unit> unitsToMove = Units.Where(u => 
                u.PlayerId == playerId &&
                u.Selected
            );

            foreach (Unit unit in unitsToMove)
            {
                unit.MoveUnit(destination);
            }
        }
    }
}

