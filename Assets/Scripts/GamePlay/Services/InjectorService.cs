using UnityEngine;
using Source.Shared.Services;
using Source.Shared.Utilities;
using System;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private InputService InputService; //CHANGE TO INTERFACE
        private GamePlayService GamePlayService; //CHANGE TO INTERFACE
        private CameraService CameraService; //CHANGE TO INTERFACE

        public void OnStart(InputService inputService, GamePlayService gameplayService, CameraService cameraService)
        {
            InputService = inputService;
            GamePlayService = gameplayService;
            CameraService = cameraService;

            this.CheckInitializeRequired();
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputService.InjectDependencies(CameraService, GamePlayService);
            GamePlayService.InjectDependencies(InputService);
        }
    }
}
