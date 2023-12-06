using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iman : Base_Weapon
{
    [Header("Unique Variables")]
    [SerializeField] ParticleEffect particleEffect;

    public override void HitOnPosition(Vector3 hitPoint)
    {
        base.HitOnPosition(hitPoint);
        if(particleEffect != null) particleEffect.particles.Play();
    }

    public override void DisableWeapon()
    {
        if (particleEffect != null) particleEffect.particles.Stop();
        base.DisableWeapon();
    }

    protected override void SpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            enemy.AlterStateBH.stateDuration = EffectDuration;
            enemy.EnemAnimator.SetTrigger("IsAlterState");
        }
    }
}
