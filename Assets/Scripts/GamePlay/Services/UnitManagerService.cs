using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Source.GamePlay.Services.Interfaces;

namespace Source.GamePlay.Services
{
    public class UnitManagerService: IUnitManagerService
    {
        public void SelectUnit(IUnitService selectedUnit, List<IUnitService> units)
        {
            foreach (UnitService unit in units.Where(u => u.Selected))
            {
                unit.Selected = false;
            }
            if (selectedUnit != null)
            {
                selectedUnit.Selected = true;
            }
        }

        public void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd, List<IUnitService> units)
        {
            IEnumerable<IUnitService> unitsToSelect = units.Where(u => 
                CheckSelectArea(u.GetPosition(), selectionStart, selectionEnd) &&
                u.PlayerId == playerId
            );

            foreach (IUnitService unit in unitsToSelect)
            {
                unit.Selected = true;
            }
        }

        private static bool CheckSelectArea(Vector3 unitPosition, Vector3 selectionStart, Vector3 selectionEnd)
        {
            if (unitPosition.x < selectionStart.x || unitPosition.x > selectionEnd.x)
            {
                return false;
            } 
            if (unitPosition.z < selectionStart.z || unitPosition.z > selectionEnd.z)
            {
                return false;
            }
            return true;
        }

        public void MoveUnits(Guid playerId, Vector3 destination, List<IUnitService> units)
        {
            IEnumerable<IUnitService> unitsToMove = units.Where(u => 
                u.PlayerId == playerId &&
                u.Selected
            );

            foreach (IUnitService unit in unitsToMove)
            {
                unit.MoveUnit(destination);
            }
        }
    }
}

