using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicMoving", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Moving/BasicMoving")]
public class EnemyBasicMoving : EnemyMovingBH
{
    //Vector3 MovemtX;
    //Vector3 MovemtY;

    public override void OnMovingEnter()
    {

    }

    public override void OnMovingExit()
    {
        Enemy.StopCoroutine(Enemy.MoveCoroutine);
    }
}
