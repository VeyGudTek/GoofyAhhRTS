using UnityEngine;

namespace Source.GamePlay.Services.Interfaces
{
    public interface ICameraService
    {
        public bool ScreenToWorldPoint(Vector2 mousePosition, out Ray ray);
        public void OnMove(Vector2 direction);
    }
}