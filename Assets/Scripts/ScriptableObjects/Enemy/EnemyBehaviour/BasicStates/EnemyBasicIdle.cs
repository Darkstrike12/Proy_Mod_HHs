using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBaicIdle", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Idle/BasicIdle")]
public class EnemyBasicIdle : EnemyIdleBH
{
    public override void OnIdleEnter()
    {
        Enemy.StartCoroutine(Enemy.MoveEnemy(Enemy.EnemyData.MovementTime));
    }
}
