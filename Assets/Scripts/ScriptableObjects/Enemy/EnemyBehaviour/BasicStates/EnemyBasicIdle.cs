using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicIdle", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Idle/BasicIdle")]
public class EnemyBasicIdle : EnemyIdleBH
{
    //float currentTime;

    public override void OnIdleEnter()
    {
        Enemy.EnemAnimator.ResetTrigger("TookDamage");
        //currentTime = 0;
        if(Enemy.Grid != null)
        {
            //Debug.Log("Grid found", Enemy);
            //Enemy.StopAllCoroutines();
            Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);
            Enemy.StartCoroutine(Enemy.MoveEnemy(Enemy.EnemyData.MovementTime));
        }
        else
        {
            Debug.Log("Grid not found", Enemy);
            Destroy(Enemy);
        }
    }

    //public override void OnIdleUpdate()
    //{
    //    currentTime += Time.deltaTime;
    //    if(currentTime > Enemy.EnemyData.MovementTime)
    //    {
    //        Enemy.GetAnimator().SetBool("IsMoving", true);
    //        Enemy.StartCoroutine(Enemy.MoveEnemy());
    //    }
    //}
}
