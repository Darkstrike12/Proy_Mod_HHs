using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base_Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] WeaponData weaponDataSO;
    public WeaponData WeaponDataSO { get { return weaponDataSO; } set { weaponDataSO = value; } }

    [Header("Weapon Behaviour Variables")]
    [SerializeField] bool IsInstantKill;
    //public bool isInstaKill { get { return IsInstantKill; } }
    [SerializeField] bool UseSpecialEffect;
    [SerializeField] float DestroyDelay;


    //Internal Referemces
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Base_Enemy Enem))
        {
            Enem.TakeDamage(WeaponDataSO.BaseDamage, IsInstantKill);
            if (UseSpecialEffect) WeaponSpecialEffect(Enem);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, DestroyDelay);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.TryGetComponent(out Base_Enemy Enem))
        //{
        //    Enem.TakeDamage(WeaponDataSO.BaseDamage, IsInstantKill);
        //    if (UseSpecialEffect) WeaponSpecialEffect(Enem);
        //    Destroy(gameObject);
        //}

        //if (collision.gameObject.TryGetComponent(out GridTile Tile))
        //{
        //    if (Tile.enemy != null)
        //    {
        //        Base_Enemy Enemy = Tile.enemy;

        //        print("Enemy Hit");
        //        Enemy.TakeDamage(WeaponDataSO.BaseDamage, IsInstantKill);
        //        if (UseSpecialEffect) WeaponSpecialEffect(Enemy);
        //        Destroy(gameObject);
        //    }
        //}
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
            var AffectedByMaterial = weaponDataSO.AffectedEnemyMaterials.Intersect(enemy.EnemyData.EnemyMaterial);
            bool AffectedByCategory = weaponDataSO.AffectedEnemyCategories.Contains(enemy.EnemyData.EnemyCategory);
            if ((AffectedByMaterial.Count() > 0 || weaponDataSO.AffectAllMaterials) && (AffectedByCategory || weaponDataSO.AffectAllCategories)) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }
}
