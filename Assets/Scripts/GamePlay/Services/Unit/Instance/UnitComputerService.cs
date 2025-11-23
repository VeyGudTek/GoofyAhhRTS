using Source.GamePlay.Services.Unit.Instance;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitComputerService : MonoBehaviour
{
    private UnitService Self { get; set; }
    public int ComputerId { get; private set; } = -1;
    private bool IsComputer => ComputerId >= 0;
    private Vector3? AgroStartLocation { get; set; } = null;
    private UnitService OriginalTarget { get; set; }
    private float OriginalStoppingDistance { get; set; }
    private Vector3 OriginalDestination { get; set; }
    private const float AgroDistance = 4;

    public void InjectDependencies(UnitService self, int? computerId)
    {
        Self = self;
        ComputerId = computerId != null ? (int)computerId : -1;
    }

    private void Update()
    {
        CheckAgroDistance();
    }

    private void CheckAgroDistance()
    {
        //This functionality is broken.
        if (AgroStartLocation != null)
        {
            if (Vector3.Distance((Vector3)AgroStartLocation, transform.position) > AgroDistance)
            {
                Self.CommandUnit(OriginalDestination, OriginalStoppingDistance, OriginalTarget);
            }
        }
    }

    public void SetOriginalCommand(Vector3 destination, float stoppingDistance, UnitService target)
    {
        OriginalDestination = destination;
        OriginalTarget = target;
        OriginalStoppingDistance = stoppingDistance;
    }

    public void OnVisionEnter(UnitService newUnitInVision)
    {
        if (!IsComputer) return;

        if (CanAgro(newUnitInVision))
        {
            AgroStartLocation = transform.position;

            Self.CommandUnit(OriginalDestination, OriginalStoppingDistance, newUnitInVision);
        }
    }

    public void OnVisionExit(List<UnitService> otherUnitsInVision)
    {
        if (!IsComputer) return;

        IEnumerable<UnitService> unitsToAgro = otherUnitsInVision.Where(u => CanAgro(u));
        if (unitsToAgro.Count() > 0)
        {
            IEnumerable<UnitService> sortedUnits = otherUnitsInVision.OrderBy(u => Vector3.Distance((Vector3)AgroStartLocation, u.transform.position));
            Self.CommandUnit(OriginalDestination, OriginalStoppingDistance, sortedUnits.First());
        }
        else
        {
            Self.CommandUnit(OriginalDestination, OriginalStoppingDistance, OriginalTarget);
            AgroStartLocation = null;
        }
    }

    private bool CanAgro(UnitService unit)
    {
        return unit.PlayerId != Self.PlayerId && !unit.UnitTypeService.IsResource;
    }
}
