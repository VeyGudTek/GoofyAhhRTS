using Moq;
using NUnit.Framework;
using Source.GamePlay.Services;
using Source.GamePlay.Services.Interfaces;

public class GamePlayServiceTests
{
    private GamePlayService GamePlayService;
    private Mock<ICameraService> _cameraService;
    private Mock<IUnitManagerService> _unitManagerService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _cameraService = new Mock<ICameraService>();
        _unitManagerService = new Mock<IUnitManagerService>();

        GamePlayService = new GamePlayService(_cameraService.Object, _unitManagerService.Object);
    }

    [Test]
    public void CantDoThisYetCuzIForgotToMockTime()
    {
        // Use the Assert class to test conditions
    }
}


