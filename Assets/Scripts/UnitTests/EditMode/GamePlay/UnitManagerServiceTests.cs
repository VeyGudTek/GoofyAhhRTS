using NUnit.Framework;
using Source.GamePlay.Services;
using System.Collections.Generic;
using Source.GamePlay.Services.Interfaces;
using Moq;
using System.Linq;


public class UnitManagerServiceTests
{
    private UnitManagerService UnitManagerService;
    private List<Mock<IUnitService>> _mockUnits;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        UnitManagerService = new UnitManagerService();
        _mockUnits = new List<Mock<IUnitService>>();

        for (int i = 0; i < 10; i++)
        {
            _mockUnits.Add(new Mock<IUnitService>());
        }
    }

    [Test]
    public void SelectUnit_SelectsOne()
    {
        foreach (Mock<IUnitService> unit in _mockUnits)
        {
            unit.SetupGet(u => u.Selected).Returns(true);
        }

        UnitManagerService.SelectUnit(_mockUnits[0].Object, _mockUnits.Select(u => u.Object).Cast<IUnitService>().ToList());

        foreach (Mock<IUnitService> unit in _mockUnits)
        {
            unit.VerifySet(u => u.Selected = false, Times.Once);
        }
        _mockUnits[0].VerifySet(u => u.Selected=true, Times.Once);
    }
}
