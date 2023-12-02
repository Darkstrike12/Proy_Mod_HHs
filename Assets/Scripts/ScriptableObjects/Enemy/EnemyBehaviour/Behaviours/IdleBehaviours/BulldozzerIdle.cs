using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulldozerIdle", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Idle/BulldozerIdle")]
public class BulldozzerIdle : EnemyIdleBH
{
    public override void OnIdleEnter()
    {
        Enemy.EnemAnimator.ResetTrigger("TookDamage");
        Enemy.SetAllowDamage(false);
        if (Enemy.Grid != null)
        {
            Enemy.StopAllCoroutines();
            Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);

            if (Enemy.EnemyData.MovementTime == 0f && Enemy.EnemyData.MovementTimeRange.Length > 0)
            {
                Enemy.MoveCoroutine = Enemy.StartCoroutine(Enemy.MoveCR(Random.Range(Enemy.EnemyData.MovementTimeRange[0], Enemy.EnemyData.MovementTimeRange[1])));
                //Enemy.StartCoroutine(Enemy.MoveCR(Random.Range(Enemy.EnemyData.MovementTimeRange[0], Enemy.EnemyData.MovementTimeRange[1])));
            }
            else
            {
                Enemy.MoveCoroutine = Enemy.StartCoroutine(Enemy.MoveCR(Enemy.EnemyData.MovementTime));
                //Enemy.StartCoroutine(Enemy.MoveCR(Enemy.EnemyData.MovementTime));
            }
        }
        else
        {
            Debug.LogError("Grid not found", Enemy);
            Destroy(Enemy);
        }
    }

    public override void OnIdleExit()
    {
        Enemy.SetAllowDamage(true);
    }
}
