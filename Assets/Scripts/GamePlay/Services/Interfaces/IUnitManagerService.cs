using System;
using UnityEngine;
using System.Collections.Generic;

namespace Source.GamePlay.Services.Interfaces
{
    public interface IUnitManagerService
    {
        void SelectUnit(UnitService selectedUnit, List<UnitService> units);
        void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd, List<UnitService> units);
        void MoveUnits(Guid playerId, Vector3 destination, List<UnitService> units);
    }
}