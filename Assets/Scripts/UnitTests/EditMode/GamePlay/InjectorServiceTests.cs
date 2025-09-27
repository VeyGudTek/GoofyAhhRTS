using NUnit.Framework;
using Source.GamePlay.Services;
using Source.Shared.HumbleObjects;
using Source.GamePlay.HumbleObjects;
using UnityEngine;

public class InjectorServiceTests
{
    private InjectorService InjectorService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        GameObject container = new GameObject();
        container.AddComponent<InputHumbleObject>();
        container.AddComponent<CameraHumbleObject>();
        InjectorService = new(container.GetComponent<InputHumbleObject>(), container.GetComponent<CameraHumbleObject>());
    }

    [Test]
    public void Update()
    {
        InjectorService.OnUpdate();
    }
}

