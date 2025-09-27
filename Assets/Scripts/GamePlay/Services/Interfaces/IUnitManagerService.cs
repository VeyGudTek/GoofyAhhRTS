using System;
using UnityEngine;
using System.Collections.Generic;

namespace Source.GamePlay.Services.Interfaces
{
    public interface IUnitManagerService
    {
        void SelectUnit(IUnitService selectedUnit, List<IUnitService> units);
        void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd, List<IUnitService> units);
        void MoveUnits(Guid playerId, Vector3 destination, List<IUnitService> units);
    }
}