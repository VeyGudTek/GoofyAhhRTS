using Moq;
using NUnit.Framework;
using Source.Shared.HumbleObjects.Interfaces;
using Source.Shared.Services.Interfaces;
using Source.Shared.Services;
using UnityEngine;
using AutoFixture;

public class InputServiceTests
{
    private InputService InputService;
    private Mock<IInputHumbleObject> _inputHumbleObject;
    private Mock<IInputProcessorService> _inputProcessorService;
    private Fixture _fixture = new Fixture();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _inputHumbleObject = new Mock<IInputHumbleObject>();
        _inputProcessorService = new Mock<IInputProcessorService>();

        InputService = new InputService(_inputHumbleObject.Object, _inputProcessorService.Object);
    }

    [SetUp]
    public void SetUp()
    {
        _inputHumbleObject.Invocations.Clear();
        _inputProcessorService.Invocations.Clear();
    }

    [Test]
    public void OnUpdate_ActiveInput()
    {
        Vector2 moveVector = _fixture.Create<Vector2>();

        _inputHumbleObject.Setup(m => m.PrimaryClicked()).Returns(true);
        _inputHumbleObject.Setup(m => m.PrimaryHold()).Returns(true);
        _inputHumbleObject.Setup(m => m.PrimaryReleased()).Returns(true);
        _inputHumbleObject.Setup(m => m.SecondaryClicked()).Returns(true);
        _inputHumbleObject.Setup(m => m.SecondaryHold()).Returns(true);
        _inputHumbleObject.Setup(m => m.SecondaryReleased()).Returns(true);
        _inputHumbleObject.Setup(m => m.GetMove()).Returns(moveVector);

        InputService.OnUpdate();

        _inputProcessorService.Verify(m => m.PrimaryClickEvent(), Times.Once());
        _inputProcessorService.Verify(m => m.PrimaryHoldEvent(), Times.Once());
        _inputProcessorService.Verify(m => m.PrimaryReleaseEvent(), Times.Once());
        _inputProcessorService.Verify(m => m.SecondaryClickEvent(), Times.Once());
        _inputProcessorService.Verify(m => m.SecondaryHoldEvent(), Times.Once());
        _inputProcessorService.Verify(m => m.SecondaryReleaseEvent(), Times.Once());
        _inputProcessorService.Verify(m => m.MoveEvent(moveVector), Times.Once());
    }

    [Test]
    public void OnUpdate_InactiveInput()
    {
        _inputHumbleObject.Setup(m => m.PrimaryClicked()).Returns(false);
        _inputHumbleObject.Setup(m => m.PrimaryHold()).Returns(false);
        _inputHumbleObject.Setup(m => m.PrimaryReleased()).Returns(false);
        _inputHumbleObject.Setup(m => m.SecondaryClicked()).Returns(false);
        _inputHumbleObject.Setup(m => m.SecondaryHold()).Returns(false);
        _inputHumbleObject.Setup(m => m.SecondaryReleased()).Returns(false);
        _inputHumbleObject.Setup(m => m.GetMove()).Returns(Vector2.zero);

        InputService.OnUpdate();

        _inputProcessorService.Verify(m => m.PrimaryClickEvent(), Times.Never());
        _inputProcessorService.Verify(m => m.PrimaryHoldEvent(), Times.Never());
        _inputProcessorService.Verify(m => m.PrimaryReleaseEvent(), Times.Never());
        _inputProcessorService.Verify(m => m.SecondaryClickEvent(), Times.Never());
        _inputProcessorService.Verify(m => m.SecondaryHoldEvent(), Times.Never());
        _inputProcessorService.Verify(m => m.SecondaryReleaseEvent(), Times.Never());
        _inputProcessorService.Verify(m => m.MoveEvent(It.IsAny<Vector2>()), Times.Never());
    }
}
