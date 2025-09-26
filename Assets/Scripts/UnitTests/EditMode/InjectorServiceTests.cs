using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using Source.GamePlay.Services;
using Source.Shared.Services.Interfaces;
using Source.GamePlay.Services.Interfaces;

namespace Source.UnitTests.Services
{
    [TestFixture]
    public class InjectorServiceTests
    {
        private InjectorService InjectorService;
        private Mock<IInputService> _inputService;
        private Mock<ICameraService> _cameraService;
        private Mock<IGamePlayService> _gamePlayService;
        private Mock<IUnitManagerService> _unitManagerService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _inputService = new Mock<IInputService>();
            _cameraService = new Mock<ICameraService>();
            _gamePlayService = new Mock<IGamePlayService>();
            _unitManagerService = new Mock<IUnitManagerService>();

            InjectorService = new(_inputService.Object, _cameraService.Object, _gamePlayService.Object, _unitManagerService.Object);
        }

        [Test]
        public void Contructor_InjectsDependencies()
        {
            _inputService.Verify(m => m.InjectDependencies(_gamePlayService.Object), Times.Once);
            _gamePlayService.Verify(m => m.InjectDependencies(_cameraService.Object, _unitManagerService.Object), Times.Once);
        }

        [Test]
        public void Update()
        {
            InjectorService.OnUpdate();

            _inputService.Verify(m => m.OnUpdate(), Times.Once);
        }
    }
}
