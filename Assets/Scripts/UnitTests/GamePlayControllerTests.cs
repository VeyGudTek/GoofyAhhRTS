using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using Source.GamePlay;

public class GamePlayControllerTests
{
    private GameObject GameObject;
    private GamePlayController GamePlayController;

    [OneTimeSetUp]
    private void SetUpGameObject()
    {
        GameObject = new GameObject();
        GamePlayController = GameObject.AddComponent<GamePlayController>();
    }

    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }
}
