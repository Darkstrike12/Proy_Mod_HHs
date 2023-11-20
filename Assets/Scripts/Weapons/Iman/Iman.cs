using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iman : Base_Weapon
{
    protected override void SpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            enemy.AlterStateBH.stateDuration = destroyDelay;
            enemy.EnemAnimator.SetTrigger("IsAlterState");
        }
    }
}
