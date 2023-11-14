using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicTakeDamage", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Take Damage/BasicTakeDamage")]
public class EnemyBasicTakeDamage : EnemyTakeDamageBH
{
    public override void OnTakeDamageEnter()
    {
        Enemy.StopAllCoroutines();
        Enemy.EnemAnimator.SetBool("IsMoving", false);
        Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);
    }
}
