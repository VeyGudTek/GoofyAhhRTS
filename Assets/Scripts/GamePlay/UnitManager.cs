using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Source.GamePlay
{
    public class UnitManager : MonoBehaviour
    {
        private List<Unit> Units = new List<Unit>();
        
        public void SelectUnit(Unit selectedUnit, bool deselectUnits = true)
        {
            foreach (Unit unit in Units.Where(u => u.Selected && deselectUnits))
            {
                unit.Deselect();
            }
            if (selectedUnit != null)
            {
                selectedUnit.Select();
            }
        }

        public void SelectUnits(Guid playerId, Vector2 selectionStart, Vector2 selectionEnd)
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

        private static bool CheckSelectArea(Vector3 unit, Vector2 selectionStart, Vector2 selectionEnd)
        {
            if (unit.x < selectionStart.x || unit.x > selectionStart.x)
            {
                return false;
            } 
            if (unit.y < selectionEnd.y || unit.y > selectionEnd.y)
            {
                return false;
            }
            return true;
        }

        public void MoveUnits(Guid playerId, Vector2 destination)
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

