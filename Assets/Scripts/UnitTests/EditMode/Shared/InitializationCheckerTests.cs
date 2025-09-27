using System;
using NUnit.Framework;
using Source.GamePlay.HumbleObjects;
using Source.GamePlay.Services;
using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializationCheckerTests
{
    private class InitializationCheckerMockObject
    {
        [InitializationRequired]
        public Guid? mockNullable;
        [InitializationRequired]
        public Camera mockComponent;
        [InitializationRequired]
        public Action mockDelegate;
        [InitializationRequired]
        public CameraHumbleObject mockHumbleObject;
        [InitializationRequired]
        public InputAction mockInput;
        [InitializationRequired]
        public CameraService mockProperty;
    }


    [Test]
    public void CheckInitializationRequired_ThrowsOnNull()
    {
        InitializationCheckerMockObject mock = new();
        InitializationException ex = Assert.Throws<InitializationException>(mock.CheckInitializeRequired);

        Assert.That(ex.Message, Is.EqualTo("[InitializationCheckerMockObject] is missing the following dependencies:\n" +
            "\t[Guid] mockNullable\n" +
            "\t[Component] mockComponent\n" +
            "\t[CallBack] mockDelegate\n" +
            "\t[Script] mockHumbleObject\n" +
            "\t[Input] mockInput\n" +
            "\t[CameraService] mockProperty\n"));
    }

    [Test]
    public void CheckInitializationRequired_PassesNonNull()
    {
        GameObject mockGameObject = new GameObject();
        Camera mockedComponent = mockGameObject.AddComponent<Camera>();
        CameraHumbleObject mockedHumbleObject = mockGameObject.AddComponent<CameraHumbleObject>();
        InitializationCheckerMockObject mock = new()
        {
            mockComponent = mockedComponent,
            mockDelegate = () => { },
            mockHumbleObject = mockedHumbleObject,
            mockInput = new InputAction(),
            mockProperty = new CameraService(mockedHumbleObject),
            mockNullable = Guid.Empty
        };

        Assert.DoesNotThrow(mock.CheckInitializeRequired);
    }
}
