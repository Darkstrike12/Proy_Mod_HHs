using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Bulldozer : Base_Enemy
{
    //protected override bool IsTileAviable(Vector3 TargetPos)
    //{
    //    return !Physics2D.OverlapCircle(TargetPos, 0.15f, DetectedLayers);
    //}

    protected override Vector3 GetAviablePosition(Axis Axis, Vector3 initialPosition, Vector3 targetPosition)
    {
        Vector3 movementDirection = (targetPosition - initialPosition).normalized;
        Vector3 currentPositoin = initialPosition + movementDirection * transform.localScale.x;

        switch (Axis)
        {
            case Axis.X:
                switch (movementDirection.x)
                {
                    case 1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.x < targetPosition.x)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                    case -1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.x > targetPosition.x)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                }
                return currentPositoin;

            case Axis.Y:
                switch (movementDirection.y)
                {
                    case 1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.y < targetPosition.y)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                    case -1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.y > targetPosition.y)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                }
                return currentPositoin;

            default:
                Debug.LogError("Only X and Y are allowed");
                return Vector3.zero;
        }
    }

    public override void TakeDamage(int DamageTaken, bool IsInstantKill)
    {
        switch (Random.Range(0,2))
        {
            case 0:
                if (AllowDamage)
                {
                    CurrentHitPoints -= DamageTaken / 2;
                    if (CurrentHitPoints <= 0 || IsInstantKill)
                    {
                        EnemyDefeated();
                    }
                    else
                    {
                        EnemAnimator.SetFloat("Damage", 1);
                        EnemAnimator.SetTrigger("TookDamage");
                    }
                }
                break;
            case 1:
                print("No Damage");
                EnemAnimator.SetFloat("Damage", 0);
                EnemAnimator.SetTrigger("TookDamage");
                break;

        }
        
    }
}
