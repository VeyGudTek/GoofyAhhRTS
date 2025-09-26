using UnityEngine;

namespace Source.GamePlay.HumbleObjects.Interfaces
{
    public interface IUnitHumbleObject
    {
        Vector3 GetPosition();
        void MoveUnit(Vector3 destination);
        void SetSpeed(float? speed);
    }
}