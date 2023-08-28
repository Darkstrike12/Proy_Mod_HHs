using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Description")]
    [SerializeField] EnemyMaterials[] EnemyMaterial;
    //[SerializeField] List<EnemyMaterials> Material;
    [SerializeField] EnemyCategories EnemyCategory;

    [Header("Enemy Movement")]
    [SerializeField] bool RandomX;
    [SerializeField] int MovementInX;
    [SerializeField] bool RandomY;
    [SerializeField] int MovementInY;
    [SerializeField] float MovementTime;

    [Header("Enemy Stats")]
    [SerializeField] int HitPoints;
    [SerializeField] int PointsGiven;

    [SerializeField] bool AllowDamage;

    public void TakeDamage(Base_Weapon weapon)
    {
        var damage = weapon.weaponData.AffectedEnemyMaterials.Intersect(EnemyMaterial);
        if(damage.Count() > 0 )
        {
            print(true);
        }
        else
        {
            print(false);
        }
    }

    void EnemyDefeated()
    {

    }

    public enum EnemyMaterials
    {
        None,
        Papel,
        Metal,
        Petroleo,
        Plastico
    }

    public enum EnemyCategories
    {
        None,
        Viviente,
        Andante,
        Consiente,
        Autoconsciente
    }
}
