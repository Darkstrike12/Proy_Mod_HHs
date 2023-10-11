using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicIdle", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Idle/BasicIdle")]
public class EnemyBasicIdle : EnemyIdleBH
{
    public override void OnIdleEnter()
    {
        Enemy.EnemAnimator.ResetTrigger("TookDamage");
        if (Enemy.Grid != null)
        {
            Enemy.StopAllCoroutines();
            Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);
            Enemy.MoveCoroutine = Enemy.StartCoroutine(Enemy.MoveEnemyCR(Enemy.EnemyData.MovementTime));
        }
        else
        {
            Debug.Log("Grid not found", Enemy);
            Destroy(Enemy);
        }
    }
}
