using Source.GamePlay.Services.Unit.Instance;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitComputerService : MonoBehaviour
{
    private UnitService Self { get; set; }
    public int ComputerId { get; private set; } = -1;
    private bool IsComputer => ComputerId >= 0;
    private Vector3? AgroStartLocation { get; set; } = null;
    private bool IsAgro => AgroStartLocation != null;
    private UnitService OriginalTarget { get; set; }
    private float OriginalStoppingDistance { get; set; }
    private Vector3 OriginalDestination { get; set; }
    private bool AgroOnCooldown { get; set; } = false;
    private const float AgroDistance = 4;
    private const float AgroResetTime = 2;

    public void InjectDependencies(UnitService self, int? computerId)
    {
        Self = self;
        ComputerId = computerId != null ? (int)computerId : -1;
    }

    private void Update()
    {
        LookForEnemy();
        ReturnToDestination();
    }

    private void LookForEnemy()
    {
        if (IsAgro || !IsComputer) return;

        IEnumerable<UnitService> unitsToAgro =  Self.UnitVisionService.VisibleUnits.Where(u => CanAgro(u)).ToList();
        if (unitsToAgro.Count() > 0)
        {
            AgroStartLocation = transform.position;
            UnitService newTarget = unitsToAgro.OrderBy(u => Vector3.Distance(transform.position, u.transform.position)).First();
            Self.CommandUnit(OriginalDestination, OriginalStoppingDistance, newTarget);
        }
    }

    private void ReturnToDestination()
    {
        if (IsAgro && IsComputer && !AgroOnCooldown)
        {
            if (Vector3.Distance((Vector3)AgroStartLocation, transform.position) > AgroDistance)
            {
                Self.CommandUnit(OriginalDestination, OriginalStoppingDistance, OriginalTarget);
                AgroOnCooldown = true;
                StartCoroutine(ResetAgro());
            }
        }
    }

    private IEnumerator ResetAgro()
    {
        yield return new WaitForSeconds(AgroResetTime);
        AgroOnCooldown = false;
        AgroStartLocation = null;
    } 

    public void SetOriginalCommand(Vector3 destination, float stoppingDistance, UnitService target)
    {
        if (!IsAgro)
        {
            OriginalDestination = destination;
            OriginalTarget = target;
            OriginalStoppingDistance = stoppingDistance;
        }
    }

    private bool CanAgro(UnitService unit)
    {
        return unit.PlayerId != Self.PlayerId && !unit.UnitTypeService.IsResource;
    }
}
