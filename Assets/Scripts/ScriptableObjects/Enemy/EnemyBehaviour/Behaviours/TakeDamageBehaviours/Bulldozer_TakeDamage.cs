using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulldozzerTakeDamage", menuName = "Scriptable Objects/Enemy/Enemy State Behaviour/Take Damage/BulldozzerTakeDamage")]
public class Bulldozer_TakeDamage : EnemyTakeDamageBH
{
    [SerializeField] FmodEvent noDamageSound;
    public override void OnTakeDamageEnter()
    {
        Enemy.StopAllCoroutines();
        Enemy.EnemAnimator.SetBool("IsMoving", false);
        Enemy.SetAllowDamage(false);
        Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);

        switch (Enemy.EnemAnimator.GetFloat("Damage"))
        {
            case 0:
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySound(noDamageSound);
                break;
            case 1:
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySound(Enemy.EnemyData.HurtSound);
                break;
            default:
                break;
        }
    }

    public override void OnTakeDamageExit()
    {
        Enemy.SetAllowDamage(true);
    }
}
