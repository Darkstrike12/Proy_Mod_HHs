using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicIdle", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Idle/BasicIdle")]
public class EnemyBasicIdle : EnemyIdleBH
{
    [Header("Parameters")]
    [SerializeField] bool useMoventTimeRange;

    public override void OnIdleEnter()
    {
        Enemy.EnemAnimator.ResetTrigger("TookDamage");
        if (Enemy.Grid != null)
        {
            Enemy.StopAllCoroutines();
            Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);
            
            if(useMoventTimeRange && Enemy.EnemyData.MovementTimeRange.Length > 0)
            {
                Enemy.MoveCoroutine = Enemy.StartCoroutine(Enemy.MoveCR(Random.Range(Enemy.EnemyData.MovementTimeRange[0], Enemy.EnemyData.MovementTimeRange[1])));
            } 
            else
            {
                Enemy.MoveCoroutine = Enemy.StartCoroutine(Enemy.MoveCR(Enemy.EnemyData.MovementTime));
            }
        }
        else
        {
            Debug.Log("Grid not found", Enemy);
            Destroy(Enemy);
        }
    }
}
