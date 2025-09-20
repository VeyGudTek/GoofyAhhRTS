using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Source.GamePlay.Services.Units.Interfaces;

namespace Source.GamePlay.Services.Units
{
    public class UnitService: IUnitService
    {
        public void SelectUnit(Unit selectedUnit, List<Unit> units)
        {
            foreach (Unit unit in units.Where(u => u.Selected))
            {
                unit.Deselect();
            }
            if (selectedUnit != null)
            {
                selectedUnit.Select();
            }
        }

        public void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd, List<Unit> units)
        {
            IEnumerable<Unit> unitsToSelect = units.Where(u => 
                CheckSelectArea(u.GetPosition(), selectionStart, selectionEnd) &&
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

        public void MoveUnits(Guid playerId, Vector3 destination, List<Unit> units)
        {
            IEnumerable<Unit> unitsToMove = units.Where(u => 
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

