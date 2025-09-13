using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Source.GamePlay
{
    public class UnitManager : MonoBehaviour
    {
        private List<Unit> Units = new List<Unit>();
        
        public void OnPrimaryClick(Unit selectedUnit)
        {
            foreach (Unit unit in Units.Where(u => u.Selected))
            {
                unit.Deselect();
            }
            selectedUnit.Select();
        }

        public void OnPrimaryHold(Vector2 selectionStart, Vector2 selectionEnd)
        {
            IEnumerable<Unit> unitsToSelect = Units.Where(u => CheckSelectArea(u.transform.position, selectionStart, selectionEnd));

            Unit SelectedUnit = Units.Where(u => u.Selected).FirstOrDefault();
            if (SelectedUnit != null)
            {
                unitsToSelect = unitsToSelect.Where(u => u.Owner == SelectedUnit.Owner);
            }

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

        public void OnSecondaryClick(Vector2 destination)
        {
            IEnumerable<Unit> unitsToMove = Units.Where(u => 
                u.Owner == UnitOwner.Player &&
                u.Selected
            );

            foreach (Unit unit in unitsToMove)
            {
                unit.MoveUnit(destination);
            }
        }
    }
}

