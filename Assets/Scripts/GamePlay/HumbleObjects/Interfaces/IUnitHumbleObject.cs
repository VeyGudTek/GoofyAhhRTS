using UnityEngine;

namespace Source.GamePlay.Controllers.Interfaces
{
    public interface IUnitHumbleObject
    {
        Vector3 GetPosition();
        void MoveUnit(Vector3 destination);
        void SetSpeed(float? speed);
    }
}