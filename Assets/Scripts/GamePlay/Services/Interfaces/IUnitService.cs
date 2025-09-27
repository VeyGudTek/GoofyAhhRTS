using System;
using UnityEngine;

namespace Source.GamePlay.Services.Interfaces
{
    public interface IUnitService
    {
        Guid PlayerId { get; }
        bool Selected { get; set; }
        void MoveUnit(Vector3 destination);
        Vector3 GetPosition();
    }
}