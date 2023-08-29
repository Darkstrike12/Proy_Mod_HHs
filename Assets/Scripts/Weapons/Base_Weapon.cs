using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Base_Enemy;

public class Base_Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] public WeaponData weaponDataSO;

    [Header("Weapon Behaviour")]
    [SerializeField] bool IsInstantKill;
    public bool isInstaKill { get { return IsInstantKill; } }
    [SerializeField] bool UseSpecialEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Base_Enemy>().TakeDamage(this);
        if (UseSpecialEffect) WeaponSpecialEffect(collision.gameObject.GetComponent<Base_Enemy>());
    }

    public virtual void WeaponSpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            print("Special Effect");
        }
    }

    private bool IsWeaponEffective(Base_Enemy enemy)
    {
        if(!weaponDataSO.AffectAllMaterials || !weaponDataSO.AffectAllCategories)
        {
            var AffectedByMaterial = weaponDataSO.AffectedEnemyMaterials.Intersect(enemy.enemyMaterial);
            bool AffectedByCategory = weaponDataSO.AffectedEnemyCategories.Contains(enemy.enemyCategory);
            if ((AffectedByMaterial.Count() > 0 || weaponDataSO.AffectAllMaterials) && (AffectedByCategory || weaponDataSO.AffectAllCategories)) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }
}
