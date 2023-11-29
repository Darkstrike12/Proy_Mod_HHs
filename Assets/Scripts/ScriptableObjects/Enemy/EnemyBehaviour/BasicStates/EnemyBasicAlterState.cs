using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicAlterState", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Alter State/BasicAlterState")]
public class EnemyBasicAlterState : EnemyAlterStateBH
{
    //public float stateDuration;
    float currentStateDuration;

    public override void OnAlterStateEnter()
    {
        currentStateDuration = 0;
        Enemy.StopAllCoroutines();
        Enemy.EnemAnimator.SetBool("IsMoving", false);
        Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);
        Debug.Log("Time " + stateDuration);
    }

    public override void OnAlterStateUpdate()
    {
        currentStateDuration += Time.deltaTime;
        //Debug.Log("Duration " + currentStateDuration);
        if (currentStateDuration >= stateDuration)
        {
            Enemy.EnemAnimator.SetTrigger("FinishAlterState");
        }
    }
}
