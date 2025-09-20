using System;
using UnityEngine;
using System.Collections.Generic;
using Source.GamePlay.Services.Units;

namespace Source.GamePlay.Services.Interfaces
{
    public interface IUnitService
    {
        void SelectUnit(Unit selectedUnit, List<Unit> units);
        void SelectUnits(Guid playerId, Vector3 selectionStart, Vector3 selectionEnd, List<Unit> units);
        void MoveUnits(Guid playerId, Vector3 destination, List<Unit> units);
    }
}