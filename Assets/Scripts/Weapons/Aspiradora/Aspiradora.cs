using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspiradora : Base_Weapon
{
    protected override void SpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            enemy.TakeDamage(0, isInstantKill);
        }
    }
}
